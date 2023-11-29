using Fanex.Letstalk.Notification.Model;
using System;
using System.Collections.Generic;

namespace Fanex.Letstalk.Notification.Params
{
	public class HttpResendMultipleParams
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public IEnumerable<ResendLetstalkModel> Data { get; set; }
	}
}
