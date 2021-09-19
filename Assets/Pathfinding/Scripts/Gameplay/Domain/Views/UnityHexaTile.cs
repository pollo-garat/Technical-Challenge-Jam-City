using System;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class UnityHexaTile : MonoBehaviour
    {
        public IObservable<HexaTile> OnTileClicked => onTileClicked;
        public UnityHexaTile CameFromNode;
        public int GCost;
        public int HCost;
        public int FCost;
        public HexaTile HexaTile { get; private set; }

        readonly ISubject<HexaTile> onTileClicked = new Subject<HexaTile>();
        Material initialMaterial;
        Material newMaterial;

        public void CalculateFCost() => FCost = GCost + HCost;

        public void Populate(HexaTile hexaTile) => 
            HexaTile = hexaTile;

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

        void OnMouseDown() => onTileClicked.OnNext(HexaTile);
    }
}
