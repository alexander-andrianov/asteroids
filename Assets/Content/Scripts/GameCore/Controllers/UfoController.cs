using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class UfoController : IDisposable
    {
        public Action<UfoView, UfoController, int> OnDestroyView;

        private readonly UfoView _ufoView;
        private readonly UfoModel _ufoModel;
        private readonly EnemyController _enemyController;

        public UfoController(UfoView ufoView, UfoModel ufoModel, EnemyController enemyController)
        {
            _ufoView = ufoView;
            _ufoModel = ufoModel;
            _enemyController = enemyController;
            
            enemyController.OnUpdatePlayerPosition += UpdateModelPosition;
            _ufoView.OnDestroy += OnDestroy;
            _ufoModel.OnPositionUpdate += UpdateViewPosition;
        }
        
        private void UpdateModelPosition(Vector2 playerPosition, float delta)
        {
            _ufoModel.UpdateUfoPosition(_ufoView.transform.position, playerPosition, delta);
        }

        private void UpdateViewPosition()
        {
            _ufoView.SetPosition(_ufoModel.Position);
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke(_ufoView, this, _ufoModel.Reward);
        }

        public void Dispose()
        {
            _enemyController.OnUpdatePlayerPosition -= UpdateModelPosition;
            _ufoView.OnDestroy -= OnDestroy;
            _ufoModel.OnPositionUpdate -= UpdateViewPosition;
            
            _ufoModel.Dispose();
        }
    }
}