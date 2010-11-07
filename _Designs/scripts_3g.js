var jpg = 1;
var gif = 3;
var f = false;
var b = new Array;
var c = new Array; var d = new Array;

/********** reload images ************/
function prel(){
	for(i=1;i<=jpg;i++)
	{
		str = '/buttons/b' + i + '_o.jpg';
		b[i-1] = new Image();
		b[i-1].src = str;
	}
	for(i=1;i<=gif;i++)
	{
		str = '/buttons/b' + i + '_o.gif';
		c[i-1] = new Image();
		c[i-1].src = str;
	}
	for(i=1;i<=gif;i++)
	{
		str = '/buttons/b' + i + '.gif';
		d[i-1] = new Image();
		d[i-1].src = str;
	}
}

/********** Show block **********/
function show(id, img, link_id, btnId) {
	if (document.getElementById(id).style.display == 'block')
	{
		document.getElementById(id).style.display = 'none';
		if (img)
			{
				document.getElementById(img).src = "/images/arr_down.gif";
				if (link_id)
					document.getElementById(link_id).className = "link2";
			}
		if (btnId)
			document.getElementById(btnId).style.display = 'block';
	}
	else
	{
		document.getElementById(id).style.display = 'block';
		if (img)
			{
				document.getElementById(img).src = "/images/arr_up.gif"
				if (link_id)
					document.getElementById(link_id).className = "link2Sel";
			}
		if (btnId)
			document.getElementById(btnId).style.display = 'none';
	}
}

/********** Show/Hide block **********/
function show_div(id) {
	if (document.getElementById(id).style.display == 'block')
		document.getElementById(id).style.display = 'none';
	else
		document.getElementById(id).style.display = 'block';
}