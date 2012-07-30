package com{
	//import com.admin.main.MainProgressImg;
	//import com.framework.ZhctSystem;
	
	import flash.display.Bitmap;
	import flash.display.DisplayObject;
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.net.URLRequest;
	
	
	/**
	*@author:颜铃璋
	*@DateModified：2010-12-8
	*@Description:加载图片
	*/
	public class LoadImage{
		public var parentObj:Sprite;
		public var parentObjW:Number = 0;
		public var parentObjH:Number = 0;
		
		public var imageLoader:Loader;
		public var bitmap:Bitmap ;
		public var completedIsAdd:Boolean;//加载成功后是否添加
		public var isGeometricScale:Boolean = false;//是否添加加载成功监听进行等比缩放
		public var isScale:Boolean = false;//是否缩放
		public var isImageProgress:Boolean = true;//是否添加图片进度条
		public var isImageCompleted:Boolean = false;
		
		//public var progressImg:MainProgressImg ; 
		
		public var maxW:Number = 0;
		public var maxH:Number = 0;
		
		public var url:String ;
		/**
		*@Description:Constructs a new LoadImage instance.
		* isImageProgress:是否添加图片进度条
		*/
		public function LoadImage(obj:Sprite,isImageProgress:Boolean=false,parentObjW:Number=0,parentObjH:Number=0){
			parentObj = obj;
			this.isImageProgress = isImageProgress;
			
			if(parentObjW == 0)
				this.parentObjW = parentObj.width;
			else
				this.parentObjW = parentObjW;
			if(parentObjH == 0)
				this.parentObjH = parentObj.height;
			else
				this.parentObjH = parentObjH;
			
			//trace(this.parentObjW+"======================"+this.parentObjH);
			
			imageLoader = new Loader();	
		}
		
		/**
		 *@Description:加载背景图片
		 * url:加载的图片路径
		 * completedIsAdd:加载成功是否添加到父容器  默认为fasle
		 */
		public function loadImg(url:String,completedIsAdd:Boolean=false):void{
			this.completedIsAdd = completedIsAdd;
			if(url == "")
				return;
			else
				this.url = url ;
			trace("----------加载图片----------");
			trace(url);
			trace("----------------------------");
			
			var req:URLRequest = new URLRequest(url);
			imageLoader.load(req);
			
			if(isImageProgress){//默认是添加进度条
				imageLoader.contentLoaderInfo.removeEventListener(ProgressEvent.PROGRESS,imageProgress);
				imageLoader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS,imageProgress);
			}else
				if(completedIsAdd)
					parentObj.addChild(imageLoader);
			
			if(isImageCompleted){
				imageLoader.contentLoaderInfo.removeEventListener(Event.COMPLETE,imageComplete);
				imageLoader.contentLoaderInfo.addEventListener(Event.COMPLETE,imageComplete);
			}
			
			imageLoader.contentLoaderInfo.removeEventListener(IOErrorEvent.IO_ERROR,errorHandler);
			imageLoader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR,errorHandler);
		}
		
		/**
		 *@Description:加载背景图片  固定父级大小加载等比缩放图片
		 * url:加载的图片路径
		 * completedIsAdd:加载成功是否添加到父容器  默认为fasle
		 * isGeometricScale:是否添加completed事件 如果为false  那么跟调用loadImg一样
		 * maxW:最大宽
		 * maxH:最大高
		 */
		public function loadImgScale(url:String,completedIsAdd:Boolean=false,isImageCompleted:Boolean=false,maxW:Number=0,maxH:Number=0,isGeometricScale:Boolean=true):void{
			this.completedIsAdd = completedIsAdd;
			this.isGeometricScale = isGeometricScale;
			this.isImageCompleted = isImageCompleted ;
			this.maxW = maxW ;
			this.maxH = maxH ;
			loadImg(url,completedIsAdd);
		}
		
		
		/**
		 *@Description:加载图片进度
		 */
		public function imageProgress(event:ProgressEvent):void{
			var total:uint = imageLoader.contentLoaderInfo.bytesTotal;
			var loaded:uint = imageLoader.contentLoaderInfo.bytesLoaded;
			var pasent:uint = loaded/total*100;
			
			/*if( progressImg == null){
				progressImg = new MainProgressImg();
				var pImgX:Number = parentObjW/2 ;//-progressImg.width/2;
				var pImgY:Number = parentObjH/2 ;//-progressImg.height/2;
				if(parentObjW == 0 ) pImgX = 0 ;
				if(parentObjH == 0) pImgY = 0 ;
				progressImg.x = pImgX ;
				progressImg.y = pImgY ;
				parentObj.addChild(progressImg);
			}
			
			if(pasent == 100){
				if(completedIsAdd && parentObj != null && !parentObj.contains(imageLoader)){
					parentObj.addChild(imageLoader);//只有当添加成功以后才能修改concent的高宽  也才能修改消除锯齿
				}
				if(progressImg != null && parentObj.contains(progressImg))
					parentObj.removeChild(progressImg);
			}*/
		}
		
		/**
		 *@Description:加载成功
		 */
		public function imageComplete(event:Event):void{
			
			
			var obj:Object = event.target.content ;
			if(maxW != 0 && maxH != 0){
				
				if(isGeometricScale){//等比缩放
					var loadImgZoom:LoadImgZoom = new LoadImgZoom(obj.width,obj.height,maxW,maxH);
					obj.width = loadImgZoom.width();
					obj.height = loadImgZoom.height();
					//obj.x = maxW/2 - loadImgZoom.width()/2 ;
					//obj.y = maxH/2 - loadImgZoom.height()/2 ;
				}else{
					obj.width = maxW;
					obj.height = maxH;
					
				}
			}
			//居中对齐
			if(parentObjW != 0)
				obj.x = parentObjW/2 - obj.width/2;
			if(parentObjH != 0)
				obj.y = parentObjH/2 - obj.height/2;
			if(completedIsAdd){
				var childIndex:Number = 0;
				if(parentObj.contains(imageLoader))	{
					childIndex = parentObj.getChildIndex(imageLoader)
					parentObj.removeChild(imageLoader);
				}
				smoothing(obj,childIndex);//消除锯齿
			}
		}
		
		private function errorHandler(e:IOErrorEvent):void{
			trace("************加载图片失败************");
			trace("找不到："+url);
			trace("************************************");
		}
		
		/**
		 *@Description:初始化所有的组件
		 */
		public function init():void{
			
		}
		
		/**
		 *@Description:消除锯齿
		 * obj ： loader.content   即bitmap对象
		 */
		public function smoothing(obj:Object,childIndex:Number):void{
			/*var cIndex:Number = removeBitmap();
			if(cIndex != 0)
				childIndex = cIndex ;*/
			if(bitmap!=null && parentObj.contains(bitmap)){
				childIndex = parentObj.getChildIndex(bitmap)
				parentObj.removeChild(bitmap);
			}
			if(parentObj.contains(imageLoader))	{
				childIndex = parentObj.getChildIndex(imageLoader)
				parentObj.removeChild(imageLoader);
			}
			bitmap = new Bitmap(obj.bitmapData,"auto",true);
			bitmap.width = obj.width ;
			bitmap.height = obj.height ;
			bitmap.x = obj.x ;
			bitmap.y = obj.y ;
			parentObj.addChildAt(bitmap,childIndex);
			
		}
		
		/**
		 *@Description:删除对象
		 * obj ： loader.content   即bitmap对象
		 */
		public function removeBitmap():Number{
			var childIndex:Number = 0 ;
			if(bitmap!=null && parentObj.contains(bitmap)){
				childIndex = parentObj.getChildIndex(bitmap)
				parentObj.removeChild(bitmap);
			}
			if(parentObj.contains(imageLoader))	{
				childIndex = parentObj.getChildIndex(imageLoader)
				parentObj.removeChild(imageLoader);
			}
			return childIndex ;
		}
	}
	
	
	
	
}