using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using M1.Core;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleSave
{
	public class ChangedEntityInfo
	{
		public List<object> AddedRows = new List<object>();

		public Dictionary<object, DataRow> ChangedRows = new Dictionary<object, DataRow>();

		public List<DataRow> DeletedRows = new List<DataRow>();

		public List<EntityDescriptor> Descriptors;
	}

	public class EntityDescriptor
	{
		public MethodInfo GetAccessor;

		public MethodInfo SetAccessor;

		public MethodInfo ParentGetAccessor;

		public string FieldName;

		public string PropertyName;

		public override string ToString()
		{
			return $"{PropertyName} => {FieldName}";
		}
	}

	public void SaveSchedule(M1Database database, ScheduleTree source)
	{
		if (source.TreeID == 0)
		{
			source.TreeID = (int)database.NextIDs.GetNextIDForTable("ScheduleTrees");
			foreach (ScheduleBranch allBranch in source.AllBranches)
			{
				allBranch.TreeID = source.TreeID;
			}
			foreach (ScheduleTask allTask in source.AllTasks)
			{
				allTask.TreeID = source.TreeID;
			}
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From ScheduleTrees Where sxtScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleBranches Where sxbScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter2;
		DataTable dataTable2 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter2);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleTasks Where sxkScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter3;
		DataTable dataTable3 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter3);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleResourceLanes Where sxrScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter4;
		DataTable dataTable4 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter4);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleAllocations Where sxdScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter5;
		DataTable dataTable5 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter5);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleResourceCells Where sxcTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter6;
		DataTable dataTable6 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter6);
		sqlCommand = database.NewSqlCommand("Select * From ScheduleTaskBuckets Where sxeScheduleTreeID = @TreeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = source.TreeID;
		SqlDataAdapter adapter7;
		DataTable dataTable7 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter7);
		new Dictionary<Guid, DataRow>();
		Dictionary<Guid, IEntityUniqueID> dictionary = new Dictionary<Guid, IEntityUniqueID>();
		dictionary.Add(source.UniqueID.Value, source);
		Dictionary<Guid, IEntityUniqueID> loadedData = GenerateGuidList(source.AllBranches.Cast<IEntityUniqueID>());
		Dictionary<Guid, IEntityUniqueID> loadedData2 = GenerateGuidList(source.AllTasks.Cast<IEntityUniqueID>());
		List<ResourceLane> loadedLanesList = new List<ResourceLane>();
		source.AllTasks.ForEach(delegate(ScheduleTask task)
		{
			loadedLanesList.AddRange(task.ResourceLanes.Values);
		});
		loadedLanesList.ForEach(delegate(ResourceLane lane)
		{
			lane.SetAllocationIDs();
		});
		Dictionary<Guid, IEntityUniqueID> loadedData3 = GenerateGuidList(loadedLanesList.Cast<IEntityUniqueID>());
		List<ScheduleAllocation> loadedAllocationList = new List<ScheduleAllocation>();
		loadedLanesList.ForEach(delegate(ResourceLane lane)
		{
			loadedAllocationList.AddRange(lane.Allocations);
		});
		Dictionary<Guid, IEntityUniqueID> loadedData4 = GenerateGuidList(loadedAllocationList.Cast<IEntityUniqueID>());
		List<LaneCell> loadedCellsList = new List<LaneCell>();
		loadedLanesList.ForEach(delegate(ResourceLane lane)
		{
			loadedCellsList.AddRange(lane.Cells.Values);
		});
		Dictionary<Guid, IEntityUniqueID> loadedData5 = GenerateGuidList(loadedCellsList.Cast<IEntityUniqueID>());
		List<ScheduleTaskBucket> loadedBucketsList = new List<ScheduleTaskBucket>();
		source.AllTasks.ForEach(delegate(ScheduleTask task)
		{
			loadedBucketsList.AddRange(task.Buckets.Values);
		});
		Dictionary<Guid, IEntityUniqueID> loadedData6 = GenerateGuidList(loadedBucketsList.Cast<IEntityUniqueID>());
		ChangedEntityInfo changes = GetChanges(dictionary, dataTable, "sxtUniqueID", typeof(ScheduleTree));
		ChangedEntityInfo changes2 = GetChanges(loadedData, dataTable2, "sxbUniqueID", typeof(ScheduleBranch));
		ChangedEntityInfo changes3 = GetChanges(loadedData2, dataTable3, "sxkUniqueID", typeof(ScheduleTask));
		ChangedEntityInfo changes4 = GetChanges(loadedData3, dataTable4, "sxrUniqueID", typeof(ResourceLane));
		ChangedEntityInfo changes5 = GetChanges(loadedData4, dataTable5, "sxdUniqueID", typeof(ScheduleAllocation));
		ChangedEntityInfo changes6 = GetChanges(loadedData5, dataTable6, "sxcUniqueID", typeof(LaneCell));
		ChangedEntityInfo changes7 = GetChanges(loadedData6, dataTable7, "sxeUniqueID", typeof(ScheduleTaskBucket));
		SaveEntities(changes, dataTable);
		SaveEntities(changes2, dataTable2);
		SaveEntities(changes3, dataTable3);
		SaveEntities(changes4, dataTable4);
		SaveEntities(changes5, dataTable5);
		SaveEntities(changes6, dataTable6);
		SaveEntities(changes7, dataTable7);
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			database.UpdateData(dataTable, adapter, sqlTransaction);
			database.UpdateData(dataTable2, adapter2, sqlTransaction);
			database.UpdateData(dataTable3, adapter3, sqlTransaction);
			database.UpdateData(dataTable4, adapter4, sqlTransaction);
			database.UpdateData(dataTable5, adapter5, sqlTransaction);
			database.UpdateData(dataTable6, adapter6, sqlTransaction);
			database.UpdateData(dataTable7, adapter7, sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
		}
		database.CommitTransaction(sqlTransaction);
	}

	protected void SaveEntities(ChangedEntityInfo changes, DataTable table)
	{
		foreach (DataRow deletedRow in changes.DeletedRows)
		{
			deletedRow.Delete();
		}
		foreach (KeyValuePair<object, DataRow> changedRow in changes.ChangedRows)
		{
			SaveEntityToRow(changedRow.Key, changes.Descriptors, changedRow.Value);
		}
		foreach (object addedRow in changes.AddedRows)
		{
			table.Rows.Add(SaveEntityToRow(addedRow, changes.Descriptors, table.NewRow()));
		}
	}

	protected Dictionary<Guid, IEntityUniqueID> GenerateGuidList(IEnumerable<IEntityUniqueID> entities)
	{
		Dictionary<Guid, IEntityUniqueID> dictionary = new Dictionary<Guid, IEntityUniqueID>();
		foreach (IEntityUniqueID entity in entities)
		{
			dictionary.Add(entity.UniqueID.Value, entity);
		}
		return dictionary;
	}

	protected ChangedEntityInfo GetChanges(Dictionary<Guid, IEntityUniqueID> loadedData, DataTable dataTable, string uniqueIDField, Type entityType)
	{
		ChangedEntityInfo changedEntityInfo = new ChangedEntityInfo();
		changedEntityInfo.Descriptors = GetProps(entityType);
		Dictionary<Guid, DataRow> dictionary = new Dictionary<Guid, DataRow>();
		foreach (DataRow row in dataTable.Rows)
		{
			dictionary.Add(row.Field<Guid>(uniqueIDField), row);
		}
		foreach (KeyValuePair<Guid, DataRow> item in dictionary)
		{
			if (loadedData.ContainsKey(item.Key))
			{
				changedEntityInfo.ChangedRows.Add(loadedData[item.Key], item.Value);
				loadedData.Remove(item.Key);
			}
			else
			{
				changedEntityInfo.DeletedRows.Add(item.Value);
			}
		}
		foreach (KeyValuePair<Guid, IEntityUniqueID> loadedDatum in loadedData)
		{
			changedEntityInfo.AddedRows.Add(loadedDatum.Value);
		}
		return changedEntityInfo;
	}

	protected List<EntityDescriptor> GetProps(Type entityType)
	{
		List<EntityDescriptor> entityDescriptors = new List<EntityDescriptor>();
		object[] customAttributes = entityType.GetCustomAttributes(typeof(TablePrefixAttribute), inherit: true);
		string prefix = ((customAttributes == null || customAttributes.Length == 0) ? string.Empty : ((TablePrefixAttribute)customAttributes[0]).Prefix);
		return GetProps(entityType, entityDescriptors, prefix, null);
	}

	protected List<EntityDescriptor> GetProps(Type entityType, List<EntityDescriptor> entityDescriptors, string prefix, MethodInfo parentGetAccessor)
	{
		PropertyInfo[] properties = entityType.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				MethodInfo[] accessors = propertyInfo.GetAccessors();
				EntityDescriptor entityDescriptor = new EntityDescriptor();
				entityDescriptor.FieldName = ((ColumnAttribute)customAttributes[0]).Name;
				if (!string.IsNullOrWhiteSpace(prefix) && !entityDescriptor.FieldName.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
				{
					entityDescriptor.FieldName = prefix + entityDescriptor.FieldName;
				}
				entityDescriptor.PropertyName = propertyInfo.Name;
				entityDescriptor.GetAccessor = accessors[0];
				if (accessors.Length > 1)
				{
					entityDescriptor.SetAccessor = accessors[1];
				}
				entityDescriptor.ParentGetAccessor = parentGetAccessor;
				entityDescriptors.Add(entityDescriptor);
			}
			else
			{
				customAttributes = propertyInfo.PropertyType.GetCustomAttributes(typeof(ComplexTypeAttribute), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					customAttributes = propertyInfo.GetCustomAttributes(typeof(ComplexTypePrefixAttribute), inherit: true);
					GetProps(prefix: prefix + ((customAttributes == null || customAttributes.Length == 0) ? string.Empty : ((ComplexTypePrefixAttribute)customAttributes[0]).Prefix), entityType: propertyInfo.PropertyType, entityDescriptors: entityDescriptors, parentGetAccessor: propertyInfo.GetAccessors()[0]);
				}
			}
		}
		return entityDescriptors;
	}

	protected DataRow SaveEntityToRow(object entity, List<EntityDescriptor> descriptors, DataRow row)
	{
		foreach (EntityDescriptor descriptor in descriptors)
		{
			object obj2;
			if (descriptor.ParentGetAccessor != null)
			{
				object obj = descriptor.ParentGetAccessor.Invoke(entity, null);
				obj2 = ((obj == null) ? Activator.CreateInstance(descriptor.GetAccessor.ReturnType) : descriptor.GetAccessor.Invoke(obj, null));
			}
			else
			{
				obj2 = descriptor.GetAccessor.Invoke(entity, null);
			}
			if (obj2 == null)
			{
				obj2 = DBNull.Value;
			}
			row[descriptor.FieldName] = obj2;
		}
		return row;
	}

	protected void LoadEntityFromRow(object entity, List<EntityDescriptor> descriptors, DataRow row)
	{
		foreach (EntityDescriptor descriptor in descriptors)
		{
			descriptor.SetAccessor.Invoke(entity, new object[1] { row[descriptor.FieldName] });
		}
	}
}
