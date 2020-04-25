/**************************************************
Trivantis (http://www.trivantis.com)
**************************************************/

var ocmOrig = document.oncontextmenu
var ocmNone = new Function( "return false" )

// Progress Object
function ObjProgress(n,a,x,y,w,h,bh,v,z,brc,bri,bkc,bki,s,r,vert,rev,auto,d) {
  this.name = n
  this.altName = a
  this.x = x
  this.y = y
  this.w = w
  this.h = h
  this.bh = bh
  this.v = v
  this.z = z
  this.isGrp = false
  this.hasOnUp = false
  this.hasOnRUp = false
  this.barColor = brc
  this.barImage = bri
  this.bgColor = bkc
  this.bgImage = bki
  this.step = s
  this.range = r
  this.currPos = 0;
  this.vert = vert
  this.reverse = rev
  this.autostart = auto
  this.barObj = null
  this.obj = this.name+"Object"
  this.alreadyActioned = false;
  this.startTime = 0
  if ( d!=null && d!="undefined" )
    this.divTag = d
  else  
    this.divTag = "div"
  eval(this.obj+"=this")
}

function ObjProgressActionGoTo( destURL, destFrame ) {
  this.objLyr.actionGoTo( destURL, destFrame );
}

function ObjProgressActionGoToNewWindow( destURL, name, props ) {
  this.objLyr.actionGoToNewWindow( destURL, name, props );
}

function ObjProgressActionPlay( ) {
  this.objLyr.actionPlay();
}

function ObjProgressActionStop( ) {
  this.objLyr.actionStop();
}

function ObjProgressActionShow( ) {
  if( this.isGrp || !this.isVisible() )
    this.onShow();
}

function ObjProgressActionHide( ) {
  if( this.isGrp || this.isVisible() )
    this.onHide();
}

function ObjProgressActionLaunch( ) {
  this.objLyr.actionLaunch();
}

function ObjProgressActionExit( ) {
  this.objLyr.actionExit();
}

function ObjProgressActionChangeContents( newImage ) {
  this.objLyr.actionChangeContents( newImage );
}

function ObjProgressActionTogglePlay( ) {
  this.objLyr.actionTogglePlay();
}

function ObjProgressActionToggleShow( ) {
  if(this.objLyr.isVisible()) this.actionHide();
  else this.actionShow();
}

function ObjProgressActionLaunch( ) {
  this.objLyr.actionLaunch();
}

function ObjProgressActionExit( ) {
  this.objLyr.actionExit();
}

function ObjProgressSizeTo( w, h ) { 
    this.w = w
    this.h = h
    this.build()
    this.activate()
    if( this.objLyr )
    	this.objLyr.clipTo( 0, w, h, 0  )
}

{ // Setup prototypes
var p=ObjProgress.prototype
p.build = ObjProgressBuild
p.init = ObjProgressInit
p.activate = ObjProgressActivate
p.up = ObjProgressUp
p.down = ObjProgressDown
p.over = ObjProgressOver
p.out = ObjProgressOut
p.capture = 0
p.onOver = new Function()
p.onOut = new Function()
p.onSelect = new Function()
p.onDown = new Function()
p.onUp = new Function()
p.onRUp = new Function()
p.onDone = new Function()
p.actionStep = ObjProgressStep
p.actionSetPos = ObjProgressSetPos
p.actionGoTo = ObjProgressActionGoTo
p.actionGoToNewWindow = ObjProgressActionGoToNewWindow
p.actionPlay = ObjProgressActionPlay
p.actionStop = ObjProgressActionStop
p.actionShow = ObjProgressActionShow
p.actionHide = ObjProgressActionHide
p.actionLaunch = ObjProgressActionLaunch
p.actionExit = ObjProgressActionExit
p.actionChangeContents = ObjProgressActionChangeContents
p.actionTogglePlay = ObjProgressActionTogglePlay
p.actionToggleShow = ObjProgressActionToggleShow
p.writeLayer = ObjProgressWriteLayer
p.onShow = ObjProgressOnShow
p.onHide = ObjProgressOnHide
p.onTimer = ObjProgressOnTimer
p.isVisible = ObjProgressIsVisible
p.isPlaying = ObjProgressIsPlaying
p.onSelChg = new Function()
p.sizeTo  = ObjProgressSizeTo
p.moveTo  = ObjProgressMoveTo
}

function ObjProgressBuild() {
  var other = ''
  if ( this.bgImage == null )
  {
     other += " border-style:solid;border-width:1px;border-color:#000000;clip:rect(0px " + this.w + "px " + this.h + "px 0px);"
     this.css = buildCSS(this.name,this.x,this.y,this.w-2,this.h-2,this.v,this.z,this.bgColor, other)
  }
  else
  {
     other += " background-image:URL('"+this.bgImage+"'); layer-background-image:URL('"+this.bgImage+"'); repeat:no;"
     this.css = buildCSS(this.name,this.x,this.y,this.w,this.h,this.v,this.z,null, other)
  }
  
  this.div = '<' + this.divTag + ' id="'+this.name + '"'
  if( ( this.hasOnUp || this.isChoice ) && !is.ns4 ) this.div += ' style="cursor:pointer;"'
  this.div += '>'
  
  this.divInt = '<a name="'+this.name+'anc"'
  this.divInt += '>'
    
  this.divInt += '<div id="'+this.name+'bar"'
  this.divInt += ' style="z-index:' +this.z +';visibility:inherit;position:relative;left:1px;'

  if( ( this.hasOnUp || this.isChoice ) && !is.ns4 ) this.divInt += 'cursor:pointer;'
  
  if ( this.vert == 1 )
 {
     var barHeight = (((this.h-2)*this.currPos)/this.range)
     if (this.reverse)
        barHeight = (this.h-2)-barHeight;
     this.divInt += 'width:' + this.bh +'px;'
     this.divInt += 'top:'+(this.h-barHeight-1) +'px;'
     this.divInt += 'height:' + barHeight + 'px;'
     this.divInt += 'clip:rect(0px '+(this.w-2)+'px '+barHeight+'px 0px);'
  }
  else
  {
     this.divInt += 'top:1px;'
     var barWidth = (((this.w-2)*this.currPos)/this.range)
     if (this.reverse)
        barWidth = (this.w-2)-barWidth;
     this.divInt += 'width:' + barWidth + 'px;'
     this.divInt += 'height:' + this.bh +'px;'
     this.divInt += 'clip:rect(0px '+(this.w-2)+'px '+ barWidth+'px 0px);'
  }
  if ( this.barImage == null )
  {
     this.divInt += 'background-color:' + this.barColor +';">&nbsp;</div>'
  }
  else
  {
     this.divInt += "background-image:URL('"+this.barImage+"');background-repeat:no-repeat;"
     this.divInt += '">&nbsp;</div>'
  }
  this.divInt += '</a>'
  this.div += '</' + this.divTag +'>\n'
}

function ObjProgressInit() {
  this.objLyr = new ObjLayer(this.name)
}

function ObjProgressActivate() {
  if( this.objLyr && this.objLyr.styObj && !this.alreadyActioned )
    if( this.v ) this.actionShow()
  if( this.capture & 4 ) {
    if (is.ns4) this.objLyr.ele.captureEvents(Event.MOUSEDOWN | Event.MOUSEUP)
    this.objLyr.ele.onmousedown = new Function("event", this.obj+".down(event); return false;")
    this.objLyr.ele.onmouseup = new Function("event", this.obj+".up(event); return false;")
  }
  if( this.capture & 1 ) this.objLyr.ele.onmouseover = new Function(this.obj+".over(); return false;")
  if( this.capture & 2 ) this.objLyr.ele.onmouseout = new Function(this.obj+".out(); return false;")
  if( is.ns5 ) this.objLyr.ele.innerHTML = this.divInt
  else this.objLyr.write( this.divInt );
  this.barObj = document.getElementById( this.name + "bar" );  

  if (this.autostart==1)
    this.actionPlay()
}

function ObjProgressDown(e) {
  if( is.ie ) e = event
  if( is.ie && !is.ieMac && e.button!=1 && e.button!=2 ) return
  if( is.ieMac && e.button != 0 ) return
  if( is.ns && !is.ns4 && e.button!=0 && e.button!=2 ) return
  if( is.ns4 && e.which!=1 && e.which!=3 ) return
  this.onSelect()
  this.onDown()
}

function ObjProgressUp(e) {
  if( is.ie ) e = event
  if( is.ie && !is.ieMac && e.button!=1 && e.button!=2 ) return
  if( is.ieMac && e.button!=0 ) return
  if( is.ns && !is.ns4 && e.button!=0 && e.button!=2 ) return
  if( is.ns4 && e.which!=1 && e.which!=3 ) return
  if( ( !is.ns4 && e.button==2 ) || ( is.ns4 && e.which==3 ) )
  {
    if( this.hasOnRUp )
    {
      document.oncontextmenu = ocmNone
      this.onRUp()
      setTimeout( "document.oncontextmenu = ocmOrig", 100)
    }
  }
  else
    this.onUp()
}

function ObjProgressActionStop( ) {
    this.startTime = 0
}

function ObjProgressIsPlaying( ) {
    if ( this.startTime == 0 )
        return false
    else
        return true
}

function ObjProgressActionPlay( ) {
    if ( this.startTime == 0 )
    {
        this.startTime = new Date().getTime()
        this.actionSetPos(0)
        setTimeout( this.name + ".onTimer()", this.step/4 )
    }
}

function ObjProgressOnTimer( ) {
    if ( this.startTime != 0 )
    {
        currTime = new Date().getTime()
        var elapsedSteps = (currTime - this.startTime)/(this.step)
        var elapsedTime = (elapsedSteps - (elapsedSteps%1))*this.step
        if ( elapsedTime)
        {
            if ( elapsedTime >= this.range )
            {
                this.actionSetPos(this.range)
                this.startTime = 0
                this.onDone()
            }
            else
            {
                this.actionSetPos(elapsedTime)
                setTimeout( this.name + ".onTimer()", this.step/4 )
            }
        }
        else
              setTimeout( this.name + ".onTimer()", this.step/4 )
    }
}

function ObjProgressActionTogglePlay( ) {
  if ( this.isPlaying() )
    this.actionStop()  
  else
    this.actionPlay()  
}

function ObjProgressStep() {
  this.actionSetPos(this.currPos + this.step)
}

function ObjProgressSetPos( newPos ) {
  this.currPos = newPos
  if ( this.currPos > this.range ) this.currPos = this.range
  if ( this.barObj != null )
  {
     if ( this.vert == 1 )
     {
         var barHeight = (((this.h-2)*this.currPos)/this.range)
         if (this.reverse)
            barHeight = (this.h-2)-barHeight;
         this.barObj.style.clip = 'rect(0px '+(this.w-2)+'px '+barHeight+'px 0px)'
         this.barObj.style.top = this.h-barHeight-1
         this.barObj.style.height = barHeight
     }
     else
     {
         var barWidth = ((this.w-2)*this.currPos)/this.range
         if (this.reverse)
            barWidth = (this.w-2)-barWidth;
         this.barObj.style.clip = 'rect(0px '+(this.w-2)+'px '+ barWidth+'px 0px)'
         this.barObj.style.width = barWidth
     }
  }
}

function ObjProgressOver() {
  this.onOver()
}

function ObjProgressOut() {
  this.onOut()
}

function ObjProgressWriteLayer( newContents ) {
  if (this.objLyr) this.objLyr.write( newContents )
}

function ObjProgressOnShow() {
  this.alreadyActioned = true;
  this.objLyr.actionShow();
}

function ObjProgressOnHide() {
  this.alreadyActioned = true;
  this.objLyr.actionHide();
}

function ObjProgressIsVisible() {
  return this.objLyr.isVisible()
}

function ObjProgressMoveTo(x,y) {
  
  // ** move the outter Div
  this.x = x
  this.y = y
  this.build()
  this.activate()
  if( this.objLyr )
  	this.objLyr.moveTo(x,y)
    
}