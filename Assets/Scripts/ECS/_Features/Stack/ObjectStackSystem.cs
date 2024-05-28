using System.Collections.Generic;
using Client;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

public class ObjectStackSystem : IEcsInitSystem, IEcsRunSystem
{
    private SharedData _data;
    private EcsWorld _world;
    
    private EcsFilter<GoFromStackObject> _goFromStackObjectsFilter;
    private EcsFilter<ObjectStackProvider> _stackFilter;
    private EcsFilter<GoToStackObject> _goToStackObjectsFilter;

    public void Init()
    {
        foreach (var idx in _stackFilter)
        {
            _stackFilter.Get1(idx).Objects = new List<EcsEntity>();
            ref var stackEntity = ref _stackFilter.GetEntity(idx);
            CreateStackGrid(ref stackEntity);
        }
    }

    public void Run()
    {
        foreach (var idx in _goToStackObjectsFilter)
        {
            ref var entity = ref _goToStackObjectsFilter.GetEntity(idx);
            ref var entityGo = ref entity.Get<GameObjectProvider>();
            ref var entityStack = ref entity.Get<ObjectCurrentStack>();
            entity.Get<StackObject>();

            entityStack.Stack.Objects.Add(entity);
            entity.Get<VelocityMoving>().Target = entityStack.Stack.Grid[entityStack.Stack.Objects.Count].transform;
            entity.Get<VelocityMoving>().Speed = 1;
            entity.Get<VelocityMoving>().Accuracy = 1;

            //entityGo.Value.transform.DOScale(Vector3.one, 0.1f).ChangeStartValue(Vector3.zero).SetEase(Ease.OutCubic); //del
            //entityGo.Value.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutCubic); //del

            entity.Del<GoToStackObject>();
        }

        foreach (var idx in _goFromStackObjectsFilter)
        {
            ref var entity = ref _goFromStackObjectsFilter.GetEntity(idx);
            ref var entityGo = ref entity.Get<GameObjectProvider>();
            ref var entityStack = ref entity.Get<ObjectCurrentStack>();
            ref var goFromStack = ref entity.Get<GoFromStackObject>();
            entity.Get<StackObject>();

            entityStack.Stack.Objects.Remove(entity);
            entity.Get<VelocityMoving>().Target = goFromStack.To;
            entity.Get<VelocityMoving>().Speed = 5;
            entity.Get<VelocityMoving>().Accuracy = 0.1f;

            //entityGo.Value.transform.DOScale(Vector3.one, 0.1f).ChangeStartValue(Vector3.zero).SetEase(Ease.OutCubic); //del
            //entityGo.Value.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutCubic); //del

            entity.Del<GoFromStackObject>();
        }
    }

    private void CreateStackGrid(ref EcsEntity entity)
    {
        ref var entityGo = ref entity.Get<GameObjectProvider>();
        ref var stack = ref entity.Get<ObjectStackProvider>();
        stack.Grid = new List<Transform>();
        var parentPos = entityGo.Value.transform.position;

        for (var z = 0; z < stack.Rows; z++)
        for (var x = 0; x < stack.Columns; x++)
        for (var y = 0; y < stack.ObjectsInColumn; y++)
        {
            var _pos = parentPos + new Vector3(x * stack.ObjectsOffset.x, y * stack.ObjectsOffset.y,
                z * stack.ObjectsOffset.z);
            var go = Object.Instantiate(_data.StaticData.PrefabData.EmptyPrefab, _pos, Quaternion.identity,
                entityGo.Value.transform);
            stack.Grid.Add(go.transform);
        }

        stack.Capacity = stack.Rows * stack.Columns * stack.ObjectsInColumn;
    }

    //private IEnumerator GiveMoneyToCharacter()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    if (!IsCanGive()) yield break;
    //    StackableItem lastItem = GetLastItem();
    //    RemoveLastItem();
    //    if (!lastItem) yield break;
    //    lastItem.transform.parent = null;
    //    lastItem.PickUp(characterAtCell.moneyStack);

    //    giveMoneyCounter--;
    //    if (giveMoneyCounter > 0)
    //        StartCoroutine(GiveMoneyToCharacter());
    //    else
    //        isRunning = false;
    //}

    //private void TryTakeMoneyFromCharacter(int _howMuch)
    //{
    //    takeMoneyCounter = _howMuch;
    //    StartCoroutine(TakeMoneyFromCharacter());
    //}

    //private IEnumerator TakeMoneyFromCharacter()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    if (!IsCanTake()) yield break;
    //    var _lastItem = characterAtCell.moneyStack.GetLastItem();
    //    characterAtCell.moneyStack.RemoveLastItem();
    //    if (!_lastItem) yield break;
    //    _lastItem.transform.parent = null;
    //    _lastItem.PickUp(this);

    //    takeMoneyCounter--;
    //    if (takeMoneyCounter > 0)
    //        StartCoroutine(TakeMoneyFromCharacter());
    //}

    //private bool IsCanGive()
    //{
    //    return characterAtCell.moneyStack.HasEmptySpace() && GetItemsCount() + 1 > 0;
    //}

    //private bool IsCanTake()
    //{
    //    return characterAtCell.moneyStack.GetItemsCount() + 1 > 0;
    //}

    //public void GiveMoneyToCharacterAtCell(Character _character, int _howMuch)
    //{
    //    characterAtCell = _character;
    //    TryGiveMoneyToCharacter(_howMuch);
    //}

    //public void TakeMoneyFromCharacterAtCell(Character _character, int _howMuch)
    //{
    //    characterAtCell = _character;
    //    TryTakeMoneyFromCharacter(_howMuch);
    //}

    //public StackableItem GetLastItem()
    //{
    //    return stackedItems[GetItemsCount()];
    //}

    //private int GetItemsCount()
    //{
    //    return stackedItems.Count - 1;
    //}

    //public void ShowItems(bool _isShow)
    //{
    //    if (_isShow)
    //        foreach (var item in stackedItems)
    //            item.gameObject.SetActive(false);
    //    else
    //        foreach (var item in stackedItems)
    //            item.gameObject.SetActive(true);
    //}

    //public void RemoveLastItem()
    //{
    //    if (stackedItems.Count > 0 && GetLastItem())
    //        stackedItems.Remove(GetLastItem());
    //}

    //public bool HasEmptySpace()
    //{
    //    return stackedItems.Count < stackLimit;
    //}
}