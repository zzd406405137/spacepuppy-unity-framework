﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy.Tween;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Scenario
{
    public class i_TweenValue : TriggerableMechanism
    {

        #region Fields

        [SerializeField()]
        private SPTime _timeSupplier;

        [SerializeField()]
        [SelectableComponent()]
        private Component _target;

        [SerializeField()]
        private TweenData[] _data;

        [SerializeField()]
        private Trigger _onComplete;

        #endregion

        #region Methods

        #endregion

        #region ITriggerable Interface

        public override bool CanTrigger
        {
            get
            {
                return base.CanTrigger && _target != null && _data.Length > 0;
            }
        }

        public override bool Trigger(object arg)
        {
            if (!this.CanTrigger) return false;

            var twn = SPTween.Tween(_target);
            for (int i = 0; i < _data.Length; i++)
            {
                twn.ByAnimMode(_data[i].Mode, _data[i].MemberName, EaseMethods.GetEase(_data[i].Ease), _data[i].ValueS.Value, _data[i].Duration, _data[i].ValueE.Value);
            }
            twn.Use(_timeSupplier.TimeSupplier);

            if (_onComplete.Count > 0)
                twn.OnFinish((t) => _onComplete.ActivateTrigger());

            twn.Play();
            return true;
        }

        #endregion

        #region Special Types

        [System.Serializable()]
        public class TweenData
        {
            [SerializeField()]
            [EnumPopupExcluding((int)TweenHash.AnimMode.AnimCurve, (int)TweenHash.AnimMode.Curve)]
            public TweenHash.AnimMode Mode;
            [SerializeField()]
            public string MemberName;
            [SerializeField()]
            public EaseStyle Ease;
            [SerializeField()]
            public VariantReference ValueS;
            [SerializeField()]
            public VariantReference ValueE;
            [SerializeField()]
            [TimeUnitsSelector()]
            public float Duration;
        }

        #endregion

    }
}