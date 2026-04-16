using ColorvisionPaintsERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorvisionPaintsERP.Services
{
    public interface IManufacturingService
    {
        // Formulations
        Task<List<Formulation>> GetFormulationsAsync(string idToken);
        Task<Formulation?> GetFormulationAsync(string id, string idToken);
        Task<string> CreateFormulationAsync(Formulation formulation, string idToken);
        Task UpdateFormulationAsync(string id, Formulation formulation, string idToken);

        // Production Orders
        Task<List<ProductionOrder>> GetProductionOrdersAsync(string idToken);
        Task<ProductionOrder?> GetProductionOrderAsync(string id, string idToken);
        Task<string> CreateProductionOrderAsync(ProductionOrder order, string idToken);
        Task UpdateOrderStatusAsync(string id, string status, string idToken);

        // Production Batches
        Task<List<ProductionBatch>> GetBatchesForOrderAsync(string orderId, string idToken);
        Task<string> CreateBatchAsync(ProductionBatch batch, string idToken);
        Task UpdateBatchAsync(string id, ProductionBatch batch, string idToken);

        // QC Inspections
        Task<List<QCInspection>> GetQCInspectionsAsync(string idToken);
        Task<QCInspection?> GetQCInspectionAsync(string id, string idToken);
        Task<string> CreateQCInspectionAsync(QCInspection inspection, string idToken);
        Task UpdateQCInspectionAsync(string id, QCInspection inspection, string idToken);

        // Production Lines
        Task<List<ProductionLine>> GetProductionLinesAsync(string idToken);
        Task<string> CreateProductionLineAsync(ProductionLine line, string idToken);
        Task UpdateProductionLineAsync(string id, ProductionLine line, string idToken);

        // Sequence ID helper
        Task<string> GetNextSequenceIdAsync(string collection, string prefix, string idToken);
    }
}
