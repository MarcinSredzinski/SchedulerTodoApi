using Business.Library.Models;
using Business.Library.Models.OperationResults;
using Business.Library.Repositories;
using Microsoft.AspNetCore.Components.Web;
using SchedulerTodo.DB;

namespace SchedulerTodo.Repositories;


public class MyTasksRepository(SqlServerDbContext dbContext) : IMyTasksRepository
{
    public OperationResult AddItemToDo(ItemToDoDto itemToDo)
    {
        var item = new ItemToDo(itemToDo);
        dbContext.ItemsToDo.Add(item);
        var affected =  dbContext.SaveChanges();
        return affected>0 ? new OperationSuccessfulResult() : new OperationResult(false, "Item not added. Please try again.");
    }
    public OperationResult DeleteItemsToDo(IEnumerable<ItemToDo> itemsToDo)
    {
        dbContext.RemoveRange(itemsToDo);
        var affected = dbContext.SaveChanges();
        return affected > 0 ? new OperationSuccessfulResult() : new OperationResult(false, "Items not deleted. Please try again.");

    }
    public OperationResult UpdateItemToDo(ItemToDo itemToDo)
    {
        var found = dbContext.ItemsToDo.Find(itemToDo.Id);
        if (found != null)
        {
            found.Name = itemToDo.Name;
            found.IsChecked = itemToDo.IsChecked;
        }
        var affected =  dbContext.SaveChanges();
        return affected>0 ? new OperationSuccessfulResult() : new OperationResult(false, "Item not updated. Please try again.");
    }

    public OperationResult MarkDone(IEnumerable<ItemToDo> itemToDo)
    {
        var found = dbContext.ItemsToDo.Where(f=> itemToDo.Select(i=> i.Id).Contains(f.Id)).ToList();
        if (found.Count != 0)
        {
            found.ForEach(f=> f.IsChecked = true);
        }
        var affected =  dbContext.SaveChanges();
        return affected>0 ? new OperationSuccessfulResult() : new OperationResult(false, "Items not updated. Please try again.");

    }

    protected IQueryable<ItemToDo> GetAll() => dbContext.ItemsToDo;
    
    public IEnumerable<ItemToDo> GetItemsToDo()
    {
        return GetAll().Where(i=> !i.IsChecked);
    }
    public IEnumerable<ItemToDo> GetItemsDone()
    {
        return GetAll().Where(i => i.IsChecked);
    }
}
