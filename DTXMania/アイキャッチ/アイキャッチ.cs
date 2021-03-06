﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SharpDX.Direct2D1;
using SharpDX.Animation;
using FDK;

namespace DTXMania.アイキャッチ
{
    /// <summary>
    ///     アイキャッチの基本クラス。
    /// </summary>
    /// <remarks>
    ///     アイキャッチとは、画面の切り替え時に、つなぎとして表示される画面を指す。
    ///     徐々に下画面を隠す「クローズ」と、徐々に下画面を表す「オープン」とがある。
    /// </remarks>
    abstract class アイキャッチ : Activity
    {
        public enum フェーズ
        {
            未定,
            クローズ,
            オープン,
            クローズ完了,
            オープン完了
        }
        public フェーズ 現在のフェーズ { get; protected set; } = フェーズ.未定;

        protected override void On活性化()
        {
            this.現在のフェーズ = フェーズ.未定;
        }
        protected override void On非活性化()
        {
            base.On非活性化();
        }

        /// <summary>
        ///     アイキャッチのクローズアニメーションを開始する。
        /// </summary>
        public virtual void クローズする( float 速度倍率 = 1.0f )
        {
            // 派生クラスでこのメソッドをオーバーライドし、
            // クローズ用のストーリーボードと変数の生成、トラジションの追加、ストーリーボードの開始コードなどを記述すること。

            this.現在のフェーズ = フェーズ.クローズ;
        }

        /// <summary>
        ///     アイキャッチのオープンアニメーションを開始する。
        /// </summary>
        public virtual void オープンする( float 速度倍率 = 1.0f )
        {
            // 派生クラスでこのメソッドをオーバーライドし、
            // オープン用のストーリーボードと変数の生成、トラジションの追加、ストーリーボードの開始コードなどを記述すること。

            this.現在のフェーズ = フェーズ.オープン;
        }

        /// <summary>
        ///     アイキャッチのアニメーションを進行し、アイキャッチ画像を描画する。
        /// </summary>
        public void 進行描画する( DeviceContext1 dc )
        {
            switch( this.現在のフェーズ )
            {
                case フェーズ.未定:
                    break;

                case フェーズ.クローズ:
                case フェーズ.クローズ完了:
                    this.進行描画する( dc, StoryboardStatus.Scheduled );
                    break;

                case フェーズ.オープン:
                case フェーズ.オープン完了:
                    this.進行描画する( dc, StoryboardStatus.Ready );
                    break;
            }
        }

        /// <summary>
        ///     派生クラスでこのメソッドをオーバーライドし、アイキャッチ画面の描画を行う。
        /// </summary>
        protected virtual void 進行描画する( DeviceContext1 dc, StoryboardStatus 描画しないStatus )
        {
            bool すべて完了 = true;

            // 派生クラスでは、ここで以下の(1)～(3)を実装すること。

            // (1) ストーリーボードが動作しているなら、すべて完了 フラグを false にする。
            //     例:
            //      if( context.ストーリーボード.Status != StoryboardStatus.Ready )
            //	        すべて完了 = false;

            // (2) アイキャッチ画面を描画する。
            //     ただし、現在のストーリーボードのステータスが 描画しないStatus であるなら、描画はしないこと。
            //     例：
            //          if( context.ストーリーボード.Status != 描画しないStatus )
            //          {
            //              // 描画処理
            //          }

            // (3) すべて完了したかどうかチェックする。
            if( すべて完了 )
            {
                if( this.現在のフェーズ == フェーズ.クローズ )
                {
                    this.現在のフェーズ = フェーズ.クローズ完了;
                }
                else if( this.現在のフェーズ == フェーズ.オープン )
                {
                    this.現在のフェーズ = フェーズ.オープン完了;
                }
            }
        }
    }
}
