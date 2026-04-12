using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext context)
        {
            try
            {
                var HasPlans = context.Plans.Any();
                var HasCategories = context.Categories.Any();
                if (HasPlans && HasCategories)
                    return false;

                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        context.Plans.AddRange(plans);
                }
                if (!HasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        context.Categories.AddRange(categories);
                }

                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Seeding Failed : {ex}"); 
                return false;
            }
        }

        #region HelperMethods
        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            // D:\Backend\Projects\GymManagementSystemSolution\GymManagement.PL\wwwroot\Files\categories.json
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            string date = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(date, options) ?? new List<T>();
        }
        #endregion
    }
}