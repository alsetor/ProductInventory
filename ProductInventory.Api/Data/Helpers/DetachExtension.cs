using Microsoft.EntityFrameworkCore;
using ProductInventory.Api.Models;

namespace ProductInventory.Api.Data.Helpers
{
    public static class DetachExtension
    {
        public static void DetachLocal<T>(this DbContext context, T? t, int entryId) 
        where T : class, IIdentifier
        {
            if (t is not null)
            {
                var local = context.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entryId));

                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }
            }
        }
    }
}
