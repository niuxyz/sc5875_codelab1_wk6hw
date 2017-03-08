using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security;
using System.Net;
using SimpleJSON;

public class Wk6Manager : MonoBehaviour {
	public string city = "";
	public string state = "";

	public string DataTime {
		get;
		private set;
	}

	public string weather{
		get;
		private set;
	}



	// Use this for initialization
	void Start () {
		
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		//Use UtilScript to write to a file in one line
		UtilScript.WriteStringToFile(Application.dataPath, "hello.txt", "hi!");

		//Use UtilScript to clone and mod a Vector3
		transform.position = UtilScript.CloneModVector3(transform.position, 1, 0, 0);

		//Use UtilScript to clone a Vector3
		Vector3 pos = UtilScript.CloneVector3(transform.position);

		//Create a JSONClass object
		JSONClass subClass = new JSONClass();

		//Add a value to subClass
		subClass["test"] = "value";

		//Create another JSONClass object, must include "using SimpleJSON" above
		JSONClass json = new JSONClass();

		//Add floats, strings, bools, even other classes to our json object
		json["x"].AsFloat = 7;
		json["y"].AsFloat = 0;
		json["z"].AsFloat = 2;
		json["name"] = "Matt";
		json["Alt Facts"].AsBool = false;
		json["sub"] = subClass;

		//Write "json" to a file using UtilScript
		UtilScript.WriteStringToFile(Application.dataPath, "file.json", json.ToString());

		//print out the string value of "json"
		Debug.Log(json);

		//Read in a file into a string using UtilScript
		string result = UtilScript.ReadStringFromFile(Application.dataPath, "file.json");

		//Parse string we read in from a file into a JSONNode
		JSONNode readJSON = JSON.Parse(result);

		//print out a value from the JSONNode
		Debug.Log(readJSON["z"].AsFloat);

		//Get the content from a webpage, in this case, a json value for the sunset time in Hawaii

		string content =  WeatherFromCity(city,state); 
		//content = "What";
		//turn string into a JSONNode
		JSONNode WeatherData = JSON.Parse(content);

		//Get the sunset time from the JSONNode
		//Change making: Change ["Astronomy"]["sunset"] to ["item"]["condition"]["text"]
		DataTime = WeatherData["query"]["results"]["channel"]["lastBuildDate"];

		//Shelley, Add Weather by the same way getting the Time
		weather = WeatherData["query"]["results"]["channel"]["item"]["condition"]["text"];
		print (weather);
		print(DataTime);
	}

	//Solve the internet, ignore all of them
	public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i=0; i<chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new System.TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}
	//Stop ignoring!!!

	string WeatherFromCity(string city, string state)
	{
		string address;
		//string jsonString;
		WebClient client = new WebClient();

		address = "https://query.yahooapis.com/v1/public/yql?q=select%20" +
			"*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" +
			city +
			"%2C%20" +
			state +
			"%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
		
		//jsonString = client.DownloadString (address);

		//return jsonString;
		return client.DownloadString(address);
	}	

	// Update is called once per frame
	void Update () {		
		string content =  WeatherFromCity(city,state); 
		//content = "What";
		//turn string into a JSONNode
		JSONNode WeatherData = JSON.Parse(content);

		//Get the sunset time from the JSONNode
		//Change making: Change ["Astronomy"]["sunset"] to ["item"]["condition"]["text"]
		DataTime = WeatherData["query"]["results"]["channel"]["lastBuildDate"];

		//Shelley, Add Weather by the same way getting the Time
		weather = WeatherData["query"]["results"]["channel"]["item"]["condition"]["text"];
	}
}
