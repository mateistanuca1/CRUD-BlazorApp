using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;
using MudBlazor;
using BlazorApp2.Components.Dialogs;

namespace BlazorApp2.Components.Pages
{
    public class CRUDBase : ComponentBase
    {
        [Inject]
        public Warehouse Warehouses { get; set; }

        [Inject]

        public IDialogService DialogService { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        public DbWarehouse newWarehouse = new DbWarehouse();
        public bool isWarehouseAdded = false;
        public List<DbWarehouse> warehousesList = new List<DbWarehouse>();
        public bool showAddForm = false;
        public bool delete = false;

        public bool ascending = true;

        public void SortTable(string columnName)
        {
            ascending = !ascending;

            switch (columnName)
            {
                case nameof(DbWarehouse.Warehouse):
                    warehousesList = ascending
                        ? warehousesList.OrderBy(w => w.Warehouse).ToList()
                        : warehousesList.OrderByDescending(w => w.Warehouse).ToList();
                    break;
                case nameof(DbWarehouse.CreatedBy):
                    warehousesList = ascending
                        ? warehousesList.OrderBy(w => w.CreatedBy).ToList()
                        : warehousesList.OrderByDescending(w => w.CreatedBy).ToList();
                    break;
                case nameof(DbWarehouse.CreatedAt):
                    warehousesList = ascending
                        ? warehousesList.OrderBy(w => w.CreatedAt).ToList()
                        : warehousesList.OrderByDescending(w => w.CreatedAt).ToList();
                    break;
                case nameof(DbWarehouse.ModifiedBy):
                    warehousesList = ascending
                        ? warehousesList.OrderBy(w => w.ModifiedBy).ToList()
                        : warehousesList.OrderByDescending(w => w.ModifiedBy).ToList();
                    break;
                case nameof(DbWarehouse.ModifiedAt):
                    warehousesList = ascending
                        ? warehousesList.OrderBy(w => w.ModifiedAt).ToList()
                        : warehousesList.OrderByDescending(w => w.ModifiedAt).ToList();
                    break;
            }

            StateHasChanged();
        }


        public void OpenAddForm()
        {
            showAddForm = !showAddForm;
            isWarehouseAdded = false;
        }

        public void CloseAddForm()
        {
            showAddForm = false;
        }

        protected override async Task OnInitializedAsync()
        {
           await GetWarehouseList();
        }

        public async Task EditWarehouse(DbWarehouse warehouse)
        {
            var parameters = new DialogParameters<EditWarehouseDialog>
            {
                { "oldwarehouse", warehouse},
                { "RefreshGrid", new Func<Task>(GetWarehouseList) }

            };
            var options = new DialogOptions() { CloseButton = true, BackgroundClass = "my-custom-class" };

            DialogService.Show<EditWarehouseDialog>($"Edit Warehouse {warehouse.Warehouse}", parameters, options);
        }

        public async Task DeleteWarehouse(DbWarehouse warehouse)
        {
            await DeleteWarehouseAsync(warehouse);
        }

        public async Task GetWarehouseList()
        {
            warehousesList = await Warehouses.invMiniWarehouse.ToListAsync();
            StateHasChanged();
        }

        public void ResetForm()
        {
            newWarehouse = new DbWarehouse();
        }


        public async Task CreateWarehouseAsync()
        {
            try
            {
                await Warehouses.invMiniWarehouse.AddAsync(newWarehouse);
                await Warehouses.SaveChangesAsync();
                isWarehouseAdded = true;

                ResetForm();
                await GetWarehouseList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding warehouse: {ex.Message}");
                isWarehouseAdded = false;
            }
        }

        public async Task DeleteWarehouseAsync(DbWarehouse Wh_to_delete)
        {
            try
            {
                delete = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this warehouse?");
                if (delete)
                {
                    Warehouses.invMiniWarehouse.Remove(Wh_to_delete);
                    await Warehouses.SaveChangesAsync();
                   await GetWarehouseList();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting warehouse: {ex.Message}");

            }
        }

    }



    public class Warehouse : DbContext
    {
        public Warehouse(DbContextOptions<Warehouse> options) : base(options) { }

        public DbSet<DbWarehouse> invMiniWarehouse { get; set; }
    }

    public class DbWarehouse
    {
        [Key]
        public Guid IdWarehouse { get; set; } = Guid.NewGuid();
        public string? Warehouse { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}