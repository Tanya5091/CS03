﻿
using System.ComponentModel;

using System.Windows;



namespace CS03.Tools

{

	internal interface ILoaderOwner : INotifyPropertyChanged

	{

		Visibility LoaderVisibility { get; set; }

		bool IsControlEnabled { get; set; }

	}

}