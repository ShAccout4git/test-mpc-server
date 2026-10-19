using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

[McpServerToolType]
public class PlaneTools
{
    [McpServerTool, Description("Get all possible statuses that a work item could be created. These statuses denote where a work item would be in a typical kanban flow. The state ids returned can be used with other tools such as creating work items.")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneAPIService planeAPIService)
    {
        var statuses = await planeAPIService.GetProjectStatesAsync();
        return JsonSerializer.Serialize(statuses);
    }

    [McpServerTool, Description("This tool allows you to create a simple work item. It does require a status Id which can be derived from the GetAllWorkItemStatuses Tool. Default should be Backlog if not supplied.")]
    public static async Task<string> CreateWorkItem(
        PlaneAPIService planeAPIService,
        [Description("The title or main headline for the work item - keep it brief")] string name,
        [Description("The detailed description of the work to be done, where appropriate include acceptance ctitria")] string description,
        [Description("The state or status id of the work item, derived from the GetAllWorkStatuses tool")] string stateId)
    {
        var item = await planeAPIService.CreateWorkItemAsync(name, description, stateId);
        return JsonSerializer.Serialize(item);
    }
}