using System;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class UnityHexaTile : MonoBehaviour
    {
        public IObservable<HexaTile> OnTileClicked => onTileClicked;

        readonly ISubject<HexaTile> onTileClicked = new Subject<HexaTile>();
        HexaTile hexaTile;
        Material initialMaterial;
        Material newMaterial;

        public void Populate(HexaTile hexaTile) => 
            this.hexaTile = hexaTile;

        public void SetInitialMaterial(Material initialMaterial)
        {
            this.initialMaterial = initialMaterial;
            SetMaterial(this.initialMaterial);
        }

        public void ResetMaterial() => SetMaterial(initialMaterial);

        public void SetNewMaterial(Material newMaterial)
        {
            this.newMaterial = newMaterial;
            SetMaterial(this.newMaterial);
        }

        void SetMaterial(Material material) => 
            GetComponentInChildren<MeshRenderer>().sharedMaterial = material;

        void OnMouseDown() => onTileClicked.OnNext(hexaTile);
    }
}
