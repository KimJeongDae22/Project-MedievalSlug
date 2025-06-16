using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public interface IRangeWeapon
    {
        /// <summary>소유자(플레이어·탑승물) 지정</summary>
        void Setup(GameObject owner);

        /// <summary>방향 벡터 기준 발사</summary>
        void Fire(Vector2 direction);

        /// <summary>스프라이트·애니메이터 on/off</summary>
        void SetActive(bool active);
    }
}