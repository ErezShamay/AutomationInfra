#!/bin/bash

# Pre- requisites:
# 1. install AWS CLI
# 2. the environemnt commands are run in must be authenticated to a role with access to the MWAA_ENVIRONMENT:
#     - AWS CLI Authentication: https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-authentication.html
#     - Role permissions: https://docs.aws.amazon.com/mwaa/latest/userguide/call-mwaa-apis-web.html#call-mwaa-apis-web-prereqs

# 3. retrieve the CLI token:

URL_BASE=https://84b0ae87-f846-41d2-9407-c7039cb71d41-vpce.c64.us-east-1.airflow.amazonaws.com/aws_mwaa/cli
MWAA_ENVIRONEMNT_NAME="Splitit-Airflow-Environment"
TOKEN=$(aws mwaa create-cli-token --name ${MWAA_ENVIRONEMNT_NAME} --query CliToken --output text)

# 4. run DAG
# DAG_ID='Reporting_prod_settlement_report_sync'
DAG_ID='Reporting_sandbox_settlement_report_sync'

DT=$(date '+%Y%m%d%H:%M:%S')
RUN_ID="${DAG_ID}__cli_triggered_dag_run${DT}"
PAYLOAD="dags trigger ${DAG_ID} --run-id ${RUN_ID}"

RESPONSE=$(curl -H "Authorization: Bearer ${TOKEN}" -H "Content-Type: text/plain" -X POST "${URL_BASE}" -d "${PAYLOAD}")
echo '########## stdout ##########'
echo $RESPONSE | jq -r '.stdout' | base64 --decode
echo '########## stderr ##########'
echo $RESPONSE | jq -r '.stderr' | base64 --decode

TODAY=$(date '+%Y-%m-%d')
PAYLOAD_STATE_ALL_JSON="dags list-runs -d ${DAG_ID} -o json -s ${TODAY}"

run_id_exists=true
while $run_id_exists; do
	run_id_exists=false
	sleep 5  # Optional: Add a delay between iterations and before starting first check
    TOKEN=$(aws mwaa create-cli-token --name ${MWAA_ENVIRONEMNT_NAME} --query CliToken --output text)
	RESPONSE_STATE=$(curl -H "Authorization: Bearer ${TOKEN}" -H "Content-Type: text/plain" -X POST "${URL_BASE}" -d "${PAYLOAD_STATE_ALL_JSON}")
	echo $RESPONSE_STATE
    STATE_STDOUT=$(echo $RESPONSE_STATE | jq -r '.stdout' | base64 --decode)
	echo $STATE_STDOUT
	for element in $(echo "$STATE_STDOUT" | jq -c '.[]'); do
		# Use jq to parse JSON and extract run_id
		run_id_elem=$(echo "$element" | jq -r '.run_id')

		# Check if the RUN_ID of relevant run matches the run_id of this element
		if [ "$run_id_elem" == "$RUN_ID" ]; then
			run_id_exists=true
			# Check run status
			echo "Filtered Object: $element"
			run_state=$(echo "$element" | jq -r '.state')
			echo $run_state
			if [[ $run_state == "success" ]]; then 
				echo "$(date '+%Y%m%d%H:%M:%S') :: Dag run is successful." 
				exit 0
			elif [[ $run_state == "failed" ]]; then 
				echo "$(date '+%Y%m%d%H:%M:%S') :: Dag run failed." 
				exit 1
			elif [[ $run_state == "queued" ]]; then 
				echo "$(date '+%Y%m%d%H:%M:%S') :: Dag run queued." 
			else 
				echo "$(date '+%Y%m%d%H:%M:%S') :: Dag run running." 
			fi
		fi
	done
	# If RUN_ID 
	if ! $run_id_exists; then
		echo "$(date '+%Y%m%d%H:%M:%S') :: ${RUN_ID} is not in list of dag runs for dag ${DAG_ID}"
		exit 1
	fi
done
# 5. further usage:
# Pass the other Airflow CLI commands in the payload to access other features. See reference: 'https://airflow.apache.org/docs/apache-airflow/stable/cli-and-env-variables-ref.html' 