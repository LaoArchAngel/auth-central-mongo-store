using System;
using System.Collections.Generic;

using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.MongoStore
{

  [Serializable]
  public class ClientPagingResult
  {
      public IEnumerable<Client> Collection { get; set; }
      public bool HasMore { get; set; }
  }

}
