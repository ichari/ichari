package com{
	/**
	 *@author:颜铃璋
	 *@DateModified：2010-12-16
	 *@Description:等比缩放
	 */
	public class LoadImgZoom{
		
		// 变量声明
		private var isZoom:Boolean;//是否缩放
		private var srcWidth:Number;//原始宽
		private var srcHeight:Number;//原始高
		private var maxWidth:Number;//限制宽
		private var maxHeight:Number;//限制高
		private var newWidth:Number;//新宽
		private var newHeight:Number;//新高
		
		public function LoadImgZoom(srcWidth:Number,srcHeight:Number,maxWidth:Number,maxHeight:Number):void{
			this.srcWidth=srcWidth;//获得原始宽度
			this.srcHeight=srcHeight;//获得原始高度
			this.maxWidth=maxWidth;//获得限定宽度
			this.maxHeight=maxHeight;//获得限定高度
			if(this.srcWidth>0 && this.srcWidth>0){//检查图片高度是否正常
				this.isZoom=true;//高宽正常，执行缩放处理
			}else{
				this.isZoom=false;//不正常，返回0
			}
			conductimg();//执行缩放算法
		}
		public function width():Number{//返回处理后的宽度，精确到2个小数点
			return Number(this.newWidth.toFixed(2));
		}
		public function height():Number{//返回处理后的高度，精确到2个小数点
			return Number(this.newHeight.toFixed(2));
		}
		private function conductimg():void{
			if(this.isZoom){//如果高宽正常，开始计算
				if(this.srcWidth/this.srcHeight>=this.maxWidth/this.maxHeight){
					//比较高宽比例，确定以宽或者是高为基准进行计算。
					if(this.srcWidth>this.maxWidth){//以宽为基准开始计算，
						//当宽度大于限定宽度，开始缩放
						this.newWidth=this.maxWidth;
						this.newHeight=(this.srcHeight*this.maxWidth)/this.srcWidth
					}else{
						//当宽度小于限定宽度，直接返回原始数值。
						this.newWidth=this.srcWidth;
						this.newHeight=this.srcHeight;
					}
				}else{
					if(this.srcHeight>this.maxHeight){//以高为基准，进行计算
						//当高度大于限定高度，开始缩放。
						this.newHeight=this.maxHeight;
						this.newWidth=(this.srcWidth*this.maxHeight)/this.srcHeight
					}else{
						//当高度小于限定高度，直接返回原始数值。
						this.newWidth=this.srcWidth;
						this.newHeight=this.srcHeight;
					}
				}
			}else{//不正常，返回0
				this.newWidth=0;
				this.newHeight=0;
			}
		}
	}

}