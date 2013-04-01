function newImage(ID,file) {
	document.getElementById(ID).src='/images/'+file;
}

function open_window(URL,name,left,top,width,height,menubar){
	 rp=window.open(URL,name,"top="+top+",left="+left+",height="+height+",width="+width+",menubar="+menubar+",scrollbars=1,resizable=1,status=1");
	 rp.focus();
	 return(false);
}



