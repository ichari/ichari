package com.group{
	import com.Config;
	import com.LoadImage;
	import com.ResultBean;
	import com.motion.GTween;
	import com.motion.GTweenTimeline;
	
	import fl.motion.easing.*;
	import fl.transitions.Tween;
	
	import flash.display.MovieClip;
	import flash.display.SimpleButton;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;

	/**
	 * 中奖面板
	 * */
	public class WinningPanel extends MovieClip{
		
		public var winningBn:SimpleButton ;
		public var application:Object ;
		public var bgGuang:MovieClip ;
		public var winningImgMc:MovieClip ;
		
		private var resultBean:ResultBean ;
		public function WinningPanel(application:Object=null,resultBean:ResultBean=null){
			this.application = application ;
			this.resultBean = resultBean ;
			
			new GTween(bgGuang,3,{rotation:360},{repeat:-1,autoRotation:false});
			winningBn.addEventListener(MouseEvent.CLICK,winningBnClick);
		}
		
		//初始化图片
		public function initWinningImg():void{
			//var sprite:Sprite = new Sprite();
			var winningImg:LoadImage = new LoadImage(winningImgMc,false,200,190);
			winningImg.loadImgScale(Config.getWinningImg(Number(resultBean.angle)),true,true,200,190);
			//sprite.x = winningImgMc.width/2 - winningImg.bitmap.width/2 ;
			//sprite.bitmap.y = winningImgMc.height/2 - winningImg.bitmap.height/2 ;
			//winningImgMc.addChild(winningImg.bitmap);
		}
		
		private function winningBnClick(e:MouseEvent):void{
			closePanel();
		}
		
		/**
		 * 关闭面板
		 * */
		public function closePanel():void{
			var x:Number = 600 ;
			var y:Number = 600 ;
			var gtween:GTween = new GTween(this,0.3,{scaleX:0.25,scaleY:0.25,x:x,y:y},{reflect:false,repeat:0,ease:Sine.easeInOut});
			gtween.addEventListener(Event.COMPLETE,removeThis);
		}
		
		public function removeThis(e:Event):void{
			this.parent.removeChild(this);
			//领奖按钮
			ExternalInterface.call("asCallJsGotoAward",resultBean.massage,resultBean.url,resultBean.target);
		}
		
		/**
		 * 打开中奖面板
		 * */
		public function openPanel():void{
			var x:Number = 690/2 - 360/2 ;
			var y:Number = 690/2 - 360/2 ;
			this.x = x + 180 ;
			this.y = y + 180 ;
			this.scaleX = 0.25 ;
			this.scaleY = 0.25 ;
			var gtween:GTween = new GTween(this,0.3,{scaleX:1,scaleY:1,x:x,y:y},{reflect:true,repeat:0,ease:Sine.easeInOut});
			gtween.addEventListener(Event.COMPLETE,openComplete);
		}
		
		private function openComplete(e:Event):void{
			//打开背景音乐
			application.backSound.soundPlay();
		}
		
	}
}