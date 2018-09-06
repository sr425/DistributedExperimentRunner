from argparse import ArgumentParser
import json
import sys
import re
import subprocess
from subprocess import Popen, PIPE, call

numberRegex = "\d+\.\d+"


def parseNumber(text):
    return re.search(numberRegex, text).group()


def getMetric(text, parameterName):
    match = re.search(parameterName + ':\s*' + numberRegex, text).group()
    return parseNumber(match)


def generateArguments(options, datasetpath):
    meta_parameters = [x for x in options['parameters']
                       if x['name'].startswith('$')]
    parameters = [x for x in options['parameters']
                  if not x['name'].startswith('$')]

    argumentList = []
    argumentList.append("--runandclose")
    path = datasetpath + options['filesPrefix']
    path = path[:-1]
    argumentList.append("-b=" + path)
    argumentList.append("--confCategory=\"" +
                        [x['stringValue']
                         for x in meta_parameters if x['name'] == "$Category"][0] + "\"")
    configuration = [
        x['stringValue'] for x in meta_parameters if x['name'] == "$Configuration"][0]
    argumentList.append("-c=\"" + configuration + "\"")
    argumentList.append("--vsset=\"Multithreading=0\"")

    for parameter in parameters:
        value = None
        if 'boolValue' in parameter:  # hasattr(parameter, 'boolValue'):
            if parameter['boolValue'] == True:
                value = str(1)
            else:
                value = str(0)
        elif 'intValue' in parameter:  # hasattr(parameter, 'intValue'):
            value = str(parameter['intValue'])
        elif 'doubleValue' in parameter:  # hasattr(parameter, 'doubleValue'):
            value = str(parameter['doubleValue'])
        else:
            value = parameter['stringValue']
        argumentList.append(
            "--vmset=\"" + parameter['name'] + "=" + value + "\"")

    argumentList.append("-i=\"" +
                        options['files']['frame10'] + ";" + options['files']['frame11'] + "\"")
    argumentList.append("-r=0")
    argumentList.append("-g=\"" + options['files']['groundtruth'] + "\"")
    argumentList.append("--vmset=\"Dummy=1\"")
    return argumentList


def run(tempfile, datasetpath):
    with open(tempfile, "r") as f:
        input_data = json.load(f)

    arguments = generateArguments(input_data, datasetpath)
    call = ["./Code/batchOF"]
    call.extend(arguments)
    output = str(subprocess.check_output(call))

    with open(tempfile, "w") as f:
        results = {}
        results["AAE"] = getMetric(output, "AAE")
        results["AEE"] = getMetric(output, "AEE")
        results["BP"] = getMetric(output, "BP")

        data = {}
        data["taskId"] = input_data["instanceTaskId"]
        data["values"] = results
        data["failed"] = False
        data["errorMessage"] = ""

        print("Finished call and metrics cmp")
        print(data)

        json_data = json.dumps(data)
        f.write(json_data)


if __name__ == "__main__":
    parser = ArgumentParser()
    parser.add_argument("-i", "--tempfile", dest="tempfile", type=str)
    parser.add_argument("-d", "--datasetpath",
                        dest="datasetpath", type=str)
    args = parser.parse_args()
    run(args.tempfile, args.datasetpath)
