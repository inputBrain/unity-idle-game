// using Model.Card;
//
// namespace Presentation.Entity
// {
//     public class EntityPresenter
//     {
//         private CardModel _cardModel;
//         private EntityView _view;
//
//         public void Init(CardModel cardModel, EntityView view)
//         {
//             _cardModel = cardModel;
//             _view = view;
//
//             UpdateCount(cardModel.Count.Value);
//             _cardModel.Count.OnValueChanged += UpdateCount;
//         }
//
//         // private void UpdateCount(int count)
//         // {
//         //     _view.SetCountText(count > 1 ? $"x{count}" : "", count > 1);
//         // }
//     }
//
// }