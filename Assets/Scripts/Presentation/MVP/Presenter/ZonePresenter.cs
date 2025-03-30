using Domain.Entities;
using Presentation.MVP.Views;

namespace Presentation.MVP.Presenter
{
    public class ZonePresenter
    {
        private Zone _zone;
        private ZoneView _zoneView;


        public void Init(Zone zone, ZoneView zoneView)
        {
            _zoneView = zoneView;
            _zone = zone;

            _zone.CurrentZone.OnValueChanged += _zoneView.UpdateZoneText;
        }
        
        
        public  void IncreaseZone()
        {
            _zone.CurrentZone.Value++;
        }        
        
        public  void DecreaseZone()
        {
            if (_zone.CurrentZone.Value <= 1)
            {
                return;
            }
            _zone.CurrentZone.Value--;
        }
    }
}