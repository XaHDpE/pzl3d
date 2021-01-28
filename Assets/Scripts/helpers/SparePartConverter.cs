using models.sparepart;
using UnityEngine;

namespace helpers
{
    public static class SparePartConverter
    {
        public static ObjectStateManager TransformToCarouselItem(Transform tr)
        {
            // tr.gameObject.AddComponent<CarouselItem>();
            return tr.gameObject.AddComponent<ObjectStateManager>();
        }
    }
}