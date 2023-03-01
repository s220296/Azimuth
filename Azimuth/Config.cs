using Raylib_cs;
using System.Numerics;

namespace Azimuth
{
	public class Config
	{
		private static Config? instance = null;

		private static string FilePath => $"{Directory.GetCurrentDirectory()}\\Assets\\config.cfg";
		
		public static void Create()
		{
			instance ??= new Config();
		}

		public static VALUE? Get<VALUE>(string _category, string _key)
		{
			if(instance == null)
			{
				Console.WriteLine("[Error] Config not yet initialised");

				return default;
			}

			Type valueType = typeof(VALUE);

			// is VALUE a Vector2?
			if(valueType == typeof(Vector2))
			{
				// Attempt to get the vector2 value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.vector2s[_category][_key], valueType);
			}
			
			// is VALUE a Vector3, Color, etc...
			if(valueType == typeof(Vector3))
			{
				// Attempt to get the vector3 value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.vector3s[_category][_key], valueType);
			}
			if(valueType == typeof(Color))
			{
				// Attempt to get the color value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.colors[_category][_key], valueType);
			}
			if(valueType == typeof(int))
			{
				// Attempt to get the int value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.ints[_category][_key], valueType);
			}
			if(valueType == typeof(float))
			{
				// Attempt to get the float value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.floats[_category][_key], valueType);
			}
			if(valueType == typeof(bool))
			{
				// Attempt to get the bool value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.bools[_category][_key], valueType);
			}
			if(valueType == typeof(string))
			{
				// Attempt to get the string value from the config, then try to change it to the correct type
				return (VALUE)Convert.ChangeType(instance.strings[_category][_key], valueType);
			}
			
			Console.WriteLine($"[Error] Attempted to get config value for type '{valueType}' with key '{_key}' in '{_category}.'");

			return default;
		}

		// ReSharper disable once InconsistentNaming
		private readonly Dictionary<string, Dictionary<string, Vector2>> vector2s;
		// ReSharper disable once InconsistentNaming
		private readonly Dictionary<string, Dictionary<string, Vector3>> vector3s;
		private readonly Dictionary<string, Dictionary<string, Color>> colors;
		private readonly Dictionary<string, Dictionary<string, int>> ints;
		private readonly Dictionary<string, Dictionary<string, float>> floats;
		private readonly Dictionary<string, Dictionary<string, bool>> bools;
		private readonly Dictionary<string, Dictionary<string, string>> strings;

		private Config()
		{
			vector2s = new Dictionary<string, Dictionary<string, Vector2>>();
			vector3s = new Dictionary<string, Dictionary<string, Vector3>>();
			colors = new Dictionary<string, Dictionary<string, Color>>();
			ints = new Dictionary<string, Dictionary<string, int>>();
			floats = new Dictionary<string, Dictionary<string, float>>();
			bools = new Dictionary<string, Dictionary<string, bool>>();
			strings = new Dictionary<string, Dictionary<string, string>>();

			Load();
		}

		private void Load()
		{
			FileInfo file = new FileInfo(FilePath);

			//somehow file has no directory
			if(file.DirectoryName == null)
				return;

			// if directory doesnt exist, create it and return
			if(!Directory.Exists(file.DirectoryName))
			{
				Directory.CreateDirectory(file.DirectoryName);

				return;
			}

			using(StreamReader reader = new StreamReader(FilePath))
			{
				string? line;
				string currentCategory = "";

				// Assign line to the next line in the file, then check if null
				// line will only be null if we reach end of the file
				// empty lines aren't considered null
				while((line = reader.ReadLine()) != null)
				{
					currentCategory = ProcessLine(line, currentCategory);
				}
			}
		}

		private string ProcessLine(string _line, string _category)
		{
			//This is a comment or empty line
			if((_line.Length > 0 && _line[0] == '#') || _line.Length == 0)
				return _category;

			string category = ProcessCategory(_line, _category);

			// If the category was changed, return the new category
			if(_category != category)
				return category;
			
			ProcessValue(_line, _category);
			
			return _category;
		}

		private void ProcessValue(string _line, string _category)
		{
			int equalIndex = _line.IndexOf('=');

			string varName = _line.Substring(0, equalIndex);
			string val = _line.Substring(equalIndex + 1, _line.Length - equalIndex - 1);

			if(val.Contains('.') && IsDecimal(val))
			{
				// Process a decimal value
				ProcessDecimal(varName, val, _category);
			}
			else
			{
				if(int.TryParse(val, out int iVal))
				{
					// this is an int
					InsertValue(varName, iVal, _category, ints);	
				}
				else if(bool.TryParse(val, out bool bVal))
				{
					// this is a bool
					InsertValue(varName, bVal, _category, bools);
				}
				else
				{
					// this is a string
					InsertValue(varName, val, _category, strings);
				}
			}
		}

		private void ProcessDecimal(string _varName, string _val, string _category)
		{
			string[] split = _val.Split(',');
			
			// Because split is a string array, split.Length equals the amount of ','
			// And because this is called when a decimal is detected
			// split.Length == 1 means there were no splits, so its just a float
			if(split.Length == 1)
			{
				InsertValue(_varName, float.Parse(split[0]), _category, floats);
			}
			else
			{
				// turns each value seperated by a comma into a float
				// Again, this is the process decimal function
				// so these have to be numbers with decimals
				float[] converted = new float[split.Length];
				for(int i = 0; i < converted.Length; i++)
					converted[i] = float.Parse(split[i]);

				if(converted.Length == 2)
				{
					InsertValue(_varName, new Vector2(converted[0], converted[1]), _category, vector2s);
				}
				else if(converted.Length == 3)
				{
					InsertValue(_varName, new Vector3(converted[0], converted[1], converted[2]), _category, vector3s);
				}
				else if(converted.Length == 4)
				{
					InsertValue(_varName, new Color((int)converted[0], (int)converted[1], (int)converted[2], (int)converted[3]), _category, colors);
				}
			}
		}
		
		private static bool IsDecimal(string _val)
		{
			// if there is a letter AND no decimal - FALSE
			// if decimal OR no letter - TRUE
			return !(_val.Any(char.IsLetter) && !_val.All(_c => _c == '.'));
		}

		private static string ProcessCategory(string _line, string _category)
		{
			return _line[0] == '[' ? _line.Substring(1, _line.Length - 2) : _category;
		}

		private static void InsertValue<VALUE_TYPE>(string _varName, VALUE_TYPE _value, string _category, Dictionary<string, Dictionary<string, VALUE_TYPE>> _values)
		{
			// check if category exists
			ValidateCategory(_category, _values);
			// adds to category
			_values[_category].Add(_varName, _value);
		}

		private static void ValidateCategory<VALUE_TYPE>(string _category, Dictionary<string, Dictionary<string, VALUE_TYPE>> _values)
		{
			// Creates category if category not exist
			if(!_values.ContainsKey(_category))
				_values.Add(_category, new Dictionary<string, VALUE_TYPE>());
		}
	}
}