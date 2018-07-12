using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
public class Utils : MonoBehaviour {

	public static bool IsOnline (){
		bool result = false;
		switch (Application.internetReachability)
		{
			case NetworkReachability.ReachableViaCarrierDataNetwork:
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				result = true;
			break;
		}
		return result;
	}

}
public static class LogPrrint {

	public static void PrintLog(this object ob,object msg){

        if (Debug.isDebugBuild)
		    Debug.Log("[" + ob.ToString() + "]: " + "\n" + msg.ToString());
	}

}