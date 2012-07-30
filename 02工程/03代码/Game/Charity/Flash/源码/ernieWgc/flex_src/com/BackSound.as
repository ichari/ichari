package com{
	import flash.events.Event;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.media.SoundLoaderContext;
	import flash.net.URLRequest;
	
	public class BackSound{
		public var _sound:Sound;
		public var channel:SoundChannel;
		public var pausePosition:int;
		public var soundFile:URLRequest;
		
		public var loops:int ;
		public var soundPath:String ;
		
		public function BackSound(soundPath:String,loops:int=9999){
			this.loops = loops ;
			this.soundPath = soundPath ;
			trace("================"+soundPath);
			
			try{
				_sound = new Sound();
				soundFile = new URLRequest(soundPath);
				var buffer:SoundLoaderContext = new SoundLoaderContext(5000);
				_sound.load(soundFile,buffer);
				
				channel = _sound.play(0,loops);
			}catch(e:Event){
				trace("加载 "+soundPath +"音乐文件失败！");
			}catch(e:Error){
				trace("加载 "+soundPath +"音乐文件失败！");
			}
			
		}
		
		/**
		 * 播出声音
		 * */
		public function soundPlay():void{
			trace("=============播放==============");
			channel=_sound.play(pausePosition,loops);
		} 
		
		/**
		 * 暂停声音
		 * */
		public function soundStop():void{
			pausePosition = channel.position;
			channel.stop();
		} 
	}
}