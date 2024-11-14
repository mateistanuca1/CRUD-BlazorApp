using BlazorApp2.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Components.Dialogs
{
    public class EditDialogBase : ComponentBase
    {
        [Inject]
        public Warehouse Warehouses { get; set; }

        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public DbWarehouse oldwarehouse { get; set; }

        [Parameter]
        public Func<Task> RefreshGrid { get; set; }

        public async Task UpdateWarehouse()
        {
            DbWarehouse Wh_to_change = await Warehouses.invMiniWarehouse.FindAsync(oldwarehouse.IdWarehouse);
            Wh_to_change.Warehouse = oldwarehouse.Warehouse;
            Wh_to_change.ModifiedBy = oldwarehouse.ModifiedBy;
            Wh_to_change.ModifiedAt = DateTime.UtcNow;

            await Warehouses.SaveChangesAsync();
            await RefreshGrid();
            MudDialog.Close(DialogResult.Ok(true));

        }
        public void Submit() => MudDialog.Close(DialogResult.Ok(true));
        public void Cancel() => MudDialog.Cancel();
    }
}
