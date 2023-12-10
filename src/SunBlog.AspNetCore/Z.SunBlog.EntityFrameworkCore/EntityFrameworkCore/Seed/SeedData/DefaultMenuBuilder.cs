using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.MenuModule.Dtos;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData
{
    public class DefaultMenuBuilder
    {
        private readonly SunBlogDbContext _context;
        public DefaultMenuBuilder(SunBlogDbContext dbContext)
        {
            _context = dbContext;
        }


        public void Create()
        {
            CreateDefaultMenu();
        }

        private void CreateDefaultMenu()
        {
            var defaultMenu = _context.Menu.IgnoreQueryFilters().ToList(); 
            if (defaultMenu == null || defaultMenu.Count == 0)
            {
                var jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Configs{Path.DirectorySeparatorChar}InitData{Path.DirectorySeparatorChar}Menu.txt");
                var pageFilterJson = File.ReadAllText(jsonFileName);
                var dynamicMenuList = JsonConvert.DeserializeObject<List<Menu>>(pageFilterJson);
                //Type type;
                //var dat =  SqlBulkCopyHelper.InnerGetDataTable(_context, dynamicMenuList);
                ////dat = dynamicMenuList;
                //var bulkCopy = SqlBulkCopyHelper.GetSqlBulkCopy(_context.Database.GetConnectionString()!, p =>
                //{
                //    p.FullTableName = "Menu";
                //});
                ////foreach (DataColumn item in dynamicMenuList!.Columns)  //Add mapping
                //{
                //    bulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                //}
                //bulkCopy.WriteToServer(dynamicMenuList);
                if (dynamicMenuList != null && dynamicMenuList.Count > 0)
                {
                    _context.Menu.AddRange(dynamicMenuList);
                    _context.SaveChanges();
                }

            }
        }

        private List<Menu> SplicingMenu(List<MenuCreateDto> mentDtos, List<Menu> menus, Guid? panentId)
        {
            foreach (var menudto in mentDtos)
            {
                var menu = new Menu()
                {
                    Id = Guid.NewGuid(),
                    RouteName = menudto.Name,
                    Component = menudto.Component,
                    ParentId = panentId,
                    Path = menudto.Path,
                    Type = menudto.Meta.Type,
                    IsKeepAlive = menudto.Meta.IsKeepAlive,
                    Icon = menudto.Meta.Icon,
                    IsVisible = !menudto.Meta.IsHide,
                    Sort = menudto.Meta.Sort,
                    Link = menudto.Meta.IsLink,
                    Name = menudto.Meta.Title,
                    IsDeleted = false
                };
                menus.Add(menu);
                if (menudto.Children != null && menudto.Children.Count > 0)
                {
                    SplicingMenu(menudto.Children, menus, menu.Id);
                }
            }
            return menus;
        }

        //public DataTable GetData(DataTable dataTable, DataTable newtable,DataTable oldtable, string Id = "")
        //{
        //    string path = Path.Combine(AppContext.BaseDirectory, $"Configs{Path.DirectorySeparatorChar}InitData");
        //    var dir = new DirectoryInfo(path);
        //    var files = dir.GetFiles("*.txt");
        //    foreach (var file in files)
        //    {
        //        using var reader = file.OpenText();
        //        string s = reader.ReadToEnd();
        //        var table = JsonConvert.DeserializeObject<DataTable>(s);
        //        var tableA = new DataTable();
        //        foreach (DataColumn column in table.Columns)
        //        {
        //            tableA.Columns.Add(column.ColumnName, column.DataType);
        //        }
        //        reader.Dispose();
        //        var df = GetData(table, tableA, table);
        //        using (StreamWriter writer = new StreamWriter(file.FullName))
        //        {
        //            var dfs = JsonConvert.SerializeObject(df);
        //            writer.Write(dfs);
        //        }
        //        // File.WriteAllText(file.FullName, JsonConvert.SerializeObject(df));
        //    }
        //    foreach (DataRow item in dataTable.Rows)
        //    {
        //        var id = item["Id"].ToString();
        //        var pand = oldtable.AsEnumerable().ToList().Where(p => p.Field<string>("ParentId") == id);
        //        item["Id"] = Guid.NewGuid().ToString();
        //        item["ParentId"] = !string.IsNullOrWhiteSpace(item["ParentId"].ToString()) ? Id : null;
        //        var bos = newtable.AsEnumerable().ToList().Where(p =>!string.IsNullOrWhiteSpace(p.Field<string>("Code")) && p.Field<string>("Code") == item["Code"].ToString());
        //        var boss = newtable.AsEnumerable().ToList().Where(p =>string.IsNullOrWhiteSpace(p.Field<string>("Code")) && p.Field<string>("Name") == item["Name"].ToString());
        //        if (bos != null && bos.Any())
        //        {
        //            continue;
        //        }
        //        if (boss != null && boss.Any())
        //        {
        //            continue;
        //        }
        //        DataRow newRow = newtable.NewRow();
        //        newRow.ItemArray = item.ItemArray;
        //        newtable.Rows.Add(newRow);
        //        if (pand != null && pand.Any())
        //        {
        //            GetData(pand.CopyToDataTable(), newtable,oldtable, item["Id"].ToString());
        //        }

        //    }

        //foreach (var item in dic)
        //{
        //    var pand = dataTable.AsEnumerable().ToList().Where(p => p.Field<string>("Id") == item.Key && !string.IsNullOrWhiteSpace(p.Field<string>("ParentId")));
        //}

        //    return newtable;
        //}




    }
}
