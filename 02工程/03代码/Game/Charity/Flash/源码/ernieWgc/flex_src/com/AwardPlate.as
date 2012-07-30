package com{
	import com.group.HuiseQuan;
	import com.group.JiangPan;
	import com.group.WinningPanel;
	import com.group.YellowQuan;
	import com.group.YuanDian;
	import com.motion.GTween;
	import com.motion.GTweenTimeline;
	import com.motion.PaoMaDengRotation;
	import com.motion.RotationTween;
	import com.motion.RotationTween1;
	
	import fl.motion.easing.*;
	import fl.transitions.Tween;
	
	import flash.display.Bitmap;
	import flash.display.MovieClip;
	import flash.display.SimpleButton;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.events.TimerEvent;
	import flash.geom.Point;
	import flash.utils.Timer;

;
	/**
	 * 奖盘 主应用程序
	 * */
	public class AwardPlate extends MovieClip{
		public var lotteryBn:SimpleButton ;//抽奖按钮
		public var application:Object ;//应用程序对象
		
		public var allAngle:Number ;
		
		public var yellowQuan:MovieClip ;//黄色圈
		public var huiseQuan:MovieClip ;//灰色圈
		public var jiangPan:MovieClip ;//奖盘
		public var gou:MovieClip ;
		public var lvdian:MovieClip ;
		
		
		
		public function AwardPlate(){
			this.addEventListener(Event.ADDED_TO_STAGE,addedStage);
		}
		
		private function addedStage(e:Event){
			lotteryBn.addEventListener(MouseEvent.CLICK,lotteryBnClick);
		}
		
		private function lotteryBnClick(e:MouseEvent):void{
			//添加点击音乐
			new BackSound(application.basePath+application.config.sound.sendErnieBn,1);
			application.button_clickHandler(e);
			//callRotation(null,360*15+30);
		}
		
		public function callRotation(obj:MovieClip,angle:Number=0):void{
			//angle = 720 ;
			
			
			var timeline:GTweenTimeline = new GTweenTimeline(null,10,null,{repeat:0,ease:Quartic.easeInOut,reflect:false});
			
			
			/*for(var i=1 ; i<25 ; i++){//绿点
				var lvdian:MovieClip = yellowQuan.getChildByName("lvdian"+i) as MovieClip ;
				var tw:GTween = new GTween(lvdian,10,{rotation:-angle},{repeat:0,autoRotation:false,ease:Quartic.easeInOut});
				timeline.addTween(0.6,tw);
				lvdianAray.push(lvdian);
			}
			*/
			lvdian = yellowQuan.getChildByName("lvdian") as MovieClip ;
			var tw:GTween = new GTween(lvdian,10,{rotation:-angle},{repeat:0,autoRotation:false,ease:Quartic.easeInOut});
			timeline.addTween(0.6,tw);
			//tempRotation = lvdian.rotation ;//记录一下原始角度
			
			//黄圈
			var huanQuan:MovieClip = yellowQuan.getChildByName("huanQuan") as MovieClip ;
			var huanQuanTween:GTween = new GTween(huanQuan,10,{rotation:-angle},{repeat:0,autoRotation:false,ease:Quartic.easeInOut});
			timeline.addTween(0.6,huanQuanTween);
			
			gou =  yellowQuan.getChildByName("gou") as MovieClip ;
//			var point:Point = yellowQuan.globalToLocal(new Point(-2.5,-312));
//			var xx = point.x ;
//			var yy = point.y ; 
//			trace(xx+"--------------------"+yy)
			//var gouTween:GTween = new GTween(gou,0.6,{rotation:30},{repeat:0,autoRotation:false,ease:Circular.easeIn,reflect:false});
			//timeline.addTween(1,gouTween);
			//跑马灯
			var paoMaStarMask:MovieClip = huiseQuan.getChildByName("paoMaStarMask") as MovieClip ;
			new PaoMaDengRotation(paoMaStarMask);
			
			//奖盘
			var jiangpanTween:GTween = new GTween(jiangPan,10,{rotation:-angle},{repeat:0,ease:Quartic.easeInOut,reflect:false,autoRotation:false});
			timeline.addTween(0.6,huanQuanTween);
			
			var timer:Timer = new Timer(11000,1);
			timer.addEventListener(TimerEvent.TIMER,sendErnie);
			timer.start();
			
			var startGouTimer:Timer = new Timer(1200,1);
			startGouTimer.addEventListener(TimerEvent.TIMER,startGouTimerFunction);
			startGouTimer.start();
			
			timeline.calculateDuration();
			//this.addEventListener(Event.ENTER_FRAME,enterFrame);
		}
		
		/**勾 启动方法
		 * */
		function startGouTimerFunction(e:TimerEvent):void{
			timerEnterFrame = new Timer(0.01,0);
			timerEnterFrame.addEventListener(TimerEvent.TIMER,enterFrame);
			timerEnterFrame.start();
			
			var startGouTimer:Timer = new Timer(10000,1);
			startGouTimer.addEventListener(TimerEvent.TIMER,stopGouTimerFunction);
			startGouTimer.start();
		}
		function stopGouTimerFunction(e:TimerEvent):void{
			tempRotation = -999 ;
			lvdian.rotation = 0 ;
			cnt = 0 ;
			timerEnterFrame.stop();
		}
		
		var gouHui:GTween = new GTween();
		var lvdianAray:Array = new Array();
		
		
		var timerEnterFrame:Timer;
		var tempRotation:Number = -99999;
		var guiziNum:int = 1 ;
		var cnt:int = 0;
		function enterFrame(e:Event):void{
			//lvdianAray[0].x = this.mouseX-320 ;
			//lvdianAray[0].y = this.mouseY-320 ;
			
			if(gou.currentFrame >= 8 || gou.currentFrame == 1){
				/*for each(var obj in lvdianAray){
					if(obj.hitTestPoint(340+35*Math.sin(zhuanHudu(gou.rotation)),25+35*Math.cos(zhuanHudu(gou.rotation)),true)){
						gou.gotoAndPlay(2);
					}
				}*/
				var guize:int = Math.abs(Math.round(tempRotation - lvdian.rotation )) ;
				
				var boo1:Boolean= lvdian.hitTestPoint(340+35*Math.sin(zhuanHudu(gou.rotation)),25+35*Math.cos(zhuanHudu(gou.rotation)),true) ;
				
				trace(lvdian.rotation);
				if(cnt > 10 ){
					boo1 = false ;
					guiziNum = 5 ;
				}
				var boo:Boolean = (lvdian.hitTestObject(gou) && guize > guiziNum);
				//boo = lvdian.hitTestObject(gou) ;
				
				//var guize:int = Math.abs((Math.round(lvdian.rotation) % 30)) ; 
				//if(guize == -29 || guize == 0){
				if(boo || boo1){
				//if(lvdian.hitTestPoint(340+35*Math.sin(zhuanHudu(gou.rotation)),25+35*Math.cos(zhuanHudu(gou.rotation)),true)){
					gou.gotoAndPlay(2);
					cnt++ ;
				}
				tempRotation = lvdian.rotation ;
			}
		}
		
		function zhuanHudu(angle:Number):Number{
			return angle*Math.PI/180 ;
		}
		
		
		function sendErnie(e:TimerEvent):void{
			var gouTween:GTween = new GTween(gou,0.6,{rotation:0},{repeat:0,autoRotation:false,ease:Quartic.easeOut});
			//timerEnterFrame.stop() ;
			application.backSound.soundPlay();
			application.sendErnie();
		}
		
		public function showAlertWinning(resultBean:ResultBean){
			var wp:WinningPanel = new WinningPanel(application,resultBean);
			wp.initWinningImg();
			this.addChild(wp);
			
			//中奖音乐
			application.backSound.soundStop();
			new BackSound(application.basePath+application.config.sound.winning,1);
			wp.openPanel();
		}
		
		public function setApplication(application:Object):void{
			this.application = application ;
			
			var sprite:Sprite = new Sprite();
			var bgImg:LoadImage = new LoadImage(sprite,false,416,418);
			bgImg.loadImgScale(application.basePath+Config.bgImg,true,true,416,418);
			sprite.x = -208 ;
			sprite.y = -205 ;
			jiangPan.addChild(sprite);
			
		}
	}
}