FAIL_STRING="Failed!"
RESULT=`cd .. && dotnet test`

if [[ $RESULT == *$FAIL_STRING* ]]
then
	exit 1
else
	exit 0
fi
