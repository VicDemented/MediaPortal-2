#region Copyright (C) 2007-2010 Team MediaPortal

/*
    Copyright (C) 2007-2010 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Collections.Generic;
using MediaPortal.Core;
using MediaPortal.Core.ClientCommunication;
using MediaPortal.Core.General;
using MediaPortal.Core.MediaManagement.DefaultItemAspects;
using MediaPortal.Core.MediaManagement.MLQueries;
using MediaPortal.UI.ServerCommunication;

namespace MediaPortal.UiComponents.Media.FilterCriteria
{
  public class FilterBySystemCriterion : SimpleMLFilterCriterion
  {
    #region Base overrides

    public FilterBySystemCriterion() : base(ProviderResourceAspect.ATTR_SYSTEM_ID)
    {
    }

    public override ICollection<FilterValue> GetAvailableValues(IEnumerable<Guid> necessaryMIATypeIds, IFilter filter)
    {
      IServerConnectionManager serverConnectionManager = ServiceRegistration.Get<IServerConnectionManager>();
      IServerController serverController = serverConnectionManager.ServerController;
      IDictionary<string, string> systemNames = new Dictionary<string, string>();
      foreach (MPClientMetadata client in serverController.GetAttachedClients())
        systemNames.Add(client.SystemId, client.LastClientName);
      systemNames.Add(serverConnectionManager.HomeServerSystemId, serverConnectionManager.LastHomeServerName);
      IContentDirectory cd = ServiceRegistration.Get<IServerConnectionManager>().ContentDirectory;
      if (cd == null)
        return new List<FilterValue>();
      HomogenousMap valueGroups = cd.GetValueGroups(_attributeType, necessaryMIATypeIds, filter);
      IList<FilterValue> result = new List<FilterValue>(valueGroups.Count);
      int numEmptyEntries = 0;
      foreach (KeyValuePair<object, object> group in valueGroups)
      {
        string name = group.Key as string ?? string.Empty;
        name = name.Trim();
        if (name == string.Empty)
          numEmptyEntries += (int) group.Value;
        else
        {
          string systemName;
          if (systemNames.TryGetValue(name, out systemName) && !string.IsNullOrEmpty(systemName))
            name = systemName;
          result.Add(new FilterValue(name,
              new RelationalFilter(_attributeType, RelationalOperator.EQ, group.Key), (int) group.Value, this));
        }
      }
      if (numEmptyEntries > 0)
        result.Insert(0, new FilterValue(VALUE_EMPTY_TITLE, new EmptyFilter(_attributeType), numEmptyEntries, this));
      return result;
    }

    public override ICollection<FilterValue> GroupValues(ICollection<Guid> necessaryMIATypeIds, IFilter filter)
    {
      return null;
    }

    #endregion
  }
}