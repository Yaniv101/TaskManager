using System;
using System.Globalization;
using System.Xml.Linq;

namespace Matrix.TaskManager.Common.Helpers
{
	public class CastHelper
	{ 
		public static DateTime CastValueToDateTime(object colValue, string format)
		{
			if (colValue == null) return DateTime.MinValue;
			DateTime returnResult;
			if (DateTime.TryParseExact(colValue.ToString(), format, CultureInfo.InvariantCulture,
					   DateTimeStyles.None, out returnResult))
				return returnResult;
			return DateTime.MinValue;
		}
		public static DateTime CastValueToDateTime(object colValue)
		{
			if (colValue == null) return DateTime.MinValue;
			DateTime returnResult;
			if (DateTime.TryParse(colValue.ToString(), out returnResult))
				return returnResult;
			return DateTime.MinValue;
		}

		public static string CastValueToDateTimeStr(DateTime? colValue, string format)
		{
			if (!colValue.HasValue)
				return string.Empty;

			return colValue.Value.ToString(format);

		}

		public static string CastValueToDateTimeStr(string colValue, string format)
		{
			if (string.IsNullOrEmpty(colValue))
				return string.Empty;

			return CastValueToDateTime(colValue).ToString(format);

		}

		public static Nullable<DateTime> CastValueToDateTimeNull(DateTime? colValue, string format)
		{
			if (!colValue.HasValue) return null;
			DateTime returnResult;
			if (DateTime.TryParseExact(colValue.ToString(), format, CultureInfo.InvariantCulture,
					   DateTimeStyles.None, out returnResult))
				return returnResult;
			return null;

		}

		public static Nullable<DateTime> CastValueToDateTimeNull(object colValue)
		{
			if (colValue == null) return null;
			DateTime returnResult;
			if (DateTime.TryParse(colValue.ToString(), out returnResult))
			{
				if (returnResult.Equals(DateTime.MinValue) || returnResult.Year == 1)
					return null;
				return returnResult;
			}

			return null;
		}

		public static object CastValueToDateTimeWithDBNull(object colValue)
		{
			if (colValue == DBNull.Value) return DBNull.Value;
			DateTime returnResult;
			if (DateTime.TryParse(colValue.ToString(), out returnResult))
			{
				if (returnResult.Year >= 1753 && returnResult.Year <= 9999)
					return returnResult;
			}
			return DBNull.Value;
		}

		public static long CastValueToLong(object colValue)
		{
			if (colValue == null) return -1;
			long returnResult;
			if (!long.TryParse(colValue.ToString(), out returnResult))
				return -1;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static string CastValueToNumberAndSlashesOnly(string value)
		{
			string result = value;
			if (!string.IsNullOrEmpty(value))
			{
				result = "";
				foreach (var item in value.ToCharArray())
				{
					if (char.IsNumber(item) || item == '.')
					{
						result += item;
					}
				}
			}

			return result;
		}
		public static float? CastValueToFloatNull(object colValue)
		{
			if (colValue == null) return null;
			float returnResult;
			if (!float.TryParse(colValue.ToString(), out returnResult))
				return null;
			return returnResult;
		}

		public static float CastValueToFloat(object colValue)
		{
			if (colValue == null) return 0;
			float returnResult;
			if (!float.TryParse(colValue.ToString(), out returnResult))
				return 0;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static double CastValueToDouble(object colValue)
		{
			if (colValue == null) return -1;
			double returnResult;
			if (!double.TryParse(colValue.ToString(), out returnResult))
				return -1;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static int CastValueToInt(object colValue)
		{
			if (colValue == null) return -1;
			int returnResult;
			if (!int.TryParse(colValue.ToString(), out returnResult))
				return -1;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static int? CastValueToIntNull(object colValue)
		{
			if (colValue == null) return null;
			int returnResult;
			if (!int.TryParse(colValue.ToString(), out returnResult))
				return null;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static DateTime LastDayDate(string mnYear)
		{
			var mnyearParts = mnYear.Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
			var month = CastValueToInt(mnyearParts[0]);
			var year = CastValueToInt(mnyearParts[1]);
			return new DateTime(year, month, DateTime.DaysInMonth(year, month));
		}

		public static DateTime FirstDayDate(string mnYear)
		{
			var mnyearParts = mnYear.Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
			var month = CastValueToInt(mnyearParts[0]);
			var year = CastValueToInt(mnyearParts[1]);
			return new DateTime(year, month, 1);
		}

		public static decimal CastValueToDecimal(object colValue)
		{
			if (colValue == null) return -1;
			decimal returnResult;
			if (!decimal.TryParse(colValue.ToString(), out returnResult))
				return -1;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static decimal? CastValueToDecimalNull(object colValue)
		{
			if (colValue == null) return null;
			decimal returnResult;
			if (!decimal.TryParse(colValue.ToString(), out returnResult))
				return null;
			return returnResult;
			//return DateTime.Parse(colValue.ToString());
		}

		public static string CastValueToStringRaw(object value)
		{
			string valueStr = CastValueToString(value);

			return valueStr.Replace("\"", "");

		}

		public static string CastValueToString(object value)
		{
			if (value != null)
			{
				if (!string.IsNullOrEmpty(value.ToString()))
				{
					return value.ToString().Replace("״", "\"").Trim();
				}
			}
			return string.Empty;
		}

		public static string SubString(string value, int len)
		{
			if (value == null) return string.Empty;
			if (value.Length > len)
			{
				return value.Substring(0, len);
			}

			return value;
		}


		public static bool CastValueToBool(object colValue)
		{
			if (colValue == null) return false;
			bool returnResult;
			if (colValue.ToString() == "1") return true;
			if (colValue.ToString() == "0") return false;

			bool.TryParse(colValue.ToString(), out returnResult);
			return returnResult;
		}
		 
		public static bool CheckIsNullOrEmpty(object param)
		{
			if (param != null)
			{
				if (param != DBNull.Value)
				{
					if (!string.IsNullOrEmpty(param.ToString()))
					{
						return false;
					}
				}
			}
			return true;
		}
		public static string CheckNULL(object param, bool isString)
		{
			if (param != null && !string.IsNullOrEmpty(param.ToString()))
			{
				if (isString)
					return String.Format("'{0}'", param.ToString().Replace("'", "''"));
				else
					return param.ToString();
			}
			else
			{
				return "NULL";
			}
		}

		public static string CastValueToQueryDateTime(DateTime colValue)
		{
			if (colValue == null || colValue == DateTime.MinValue)
				return "NULL";
			return "'" + colValue.Year + "-" + colValue.Month + "-" + colValue.Day + "'";
		}

		public static string ParsePhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
				return phone.Replace("-", "");

			return string.Empty;
		}
		public static string CastBitToInt(object p)
		{
			bool value = CastValueToBool(p);
			if (value)
				return "1";

			return "0";
		}

		public static int CastBoolToInt(object accessRight)
		{
			bool value = CastValueToBool(accessRight);
			if (value)
				return 1;

			return 0;
		}

		public static string AddLeadingZero(string str, int len)
		{
			for (int i = 0; i < len; i++)
			{
				if (str.Length < len)
				{
					str = "0" + str;
				}
				else
					break;
			}
			return str.Substring(0, len);
		}

		public static Nullable<DateTime> CastXmlNodeToDateValue(XElement xElement)
		{
			if (xElement != null)
			{
				if (!string.IsNullOrEmpty(xElement.Value))
					return DateTime.Parse(xElement.Value);
			}
			return null;
		}

		public static string CastXmlNodeToTextValue(XElement xElement)
		{
			if (xElement != null)
			{
				return xElement.Value;
			}
			return string.Empty;
		}


		public static string CastXmlAttToTextValue(XAttribute xAttribute)
		{
			if (xAttribute != null)
			{
				return xAttribute.Value;
			}
			return string.Empty;
		}
		public static string AddSpaces(string str, int len)
		{
			for (int i = 0; i < len; i++)
			{
				if (str.Length < len)
				{
					str += " ";
				}
				else
					break;
			}
			return str.Substring(0, len);
		}

		public static string AddLeadingSpace(string str, int len)
		{
			for (int i = 0; i < len; i++)
			{
				if (str.Length < len)
				{
					str = " " + str;
				}
				else
					break;
			}
			return str.Substring(0, len);
		}

		public static DateTime? CastJsonDateTime(long p)
		{
			try
			{
				return new DateTime(p * 10000);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static DateTime? CastJsonDateTime2(long p)
		{
			try
			{
				if (p == 0)
					return null;
				var epoc = new DateTime(1970, 1, 1);
				long delta = long.Parse((epoc.Ticks + p * 10000).ToString());

				var result = new DateTime(delta);

				return result;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static TimeSpan? CastValueToTimeSpan(object colValue)
		{

			try
			{
				if (colValue == null) return TimeSpan.MinValue;
				TimeSpan returnResult;
				if (TimeSpan.TryParse(colValue.ToString(), out returnResult))
					return returnResult;
				return TimeSpan.MinValue;

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Nullable<DateTime> CastValueToDateTime2(string dtMonthYear)
		{

			if (!string.IsNullOrEmpty(dtMonthYear))
			{
				var dtParts = dtMonthYear.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				if (dtParts.Length == 2)
				{
					var year = CastHelper.CastValueToInt(dtParts[1]);
					int month = -1;
					switch (dtParts[0])
					{
						case "ינואר":
							month = 1;
							break;
						case "פברואר":
							month = 2;
							break;
						case "מרץ":
							month = 3;
							break;
						case "אפריל":
							month = 4;
							break;
						case "מאי":
							month = 5;
							break;
						case "יוני":
							month = 6;
							break;
						case "יולי":
							month = 7;
							break;
						case "אוגוסט":
							month = 8;
							break;
						case "ספטמבר":
							month = 9;
							break;
						case "אוקטובר":
							month = 10;
							break;
						case "נובמבר":
							month = 11;
							break;
						case "דצמבר":
							month = 12;
							break;
						default:
							break;
					}
					if (month > -1)
					{
						return new DateTime(year, month, 1);
					}
				}
			}
			return null;
		}

		public static string FixCsv(string val)
		{
			if (val.Contains(","))
			{
				return val.Replace(",", " ");
			}
			return val;

		}
	}

}
