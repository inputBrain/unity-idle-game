using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class ZonePresenter
    {
        private readonly Zone _zone;
        private readonly ZoneView _zoneView;


        public ZonePresenter(ZoneView zoneView, Zone zone)
        {
            _zoneView = zoneView;
            _zone = zone;

            _zone.OnCurrenZoneChanged += _zoneView.UpdateZoneText;
        }
        
        
        public  void IncreaseZone()
        {
            _zone.CurrentZone++;
        }        
        
        public  void DecreaseZone()
        {
            if (_zone.CurrentZone <= 1)
            {
                return;
            }
            _zone.CurrentZone--;
        }
    }
}