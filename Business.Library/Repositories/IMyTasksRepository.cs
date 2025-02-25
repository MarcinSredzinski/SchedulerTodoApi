using Business.Library.Models;
using Business.Library.Models.OperationResults;

namespace Business.Library.Repositories;

public interface IMyTasksRepository
{
    OperationResult AddItemToDo(ItemToDoDto itemToDo);
    OperationResult DeleteItemsToDo(IEnumerable<ItemToDo> itemsToDo);
    IEnumerable<ItemToDo> GetItemsToDo();
    IEnumerable<ItemToDo> GetItemsDone();
    OperationResult MarkDone(IEnumerable<ItemToDo> itemToDo);
    OperationResult UpdateItemToDo(ItemToDo itemToDo);
}