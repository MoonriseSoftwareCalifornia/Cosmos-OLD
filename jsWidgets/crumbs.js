//
// This example script builds "bread crumb" navigation.
//
 // Begin bread crumb navigation builder

 document.addEventListener("DOMContentLoaded", function () {
        
    const breadCrumbs = document.getElementById(breadCrumbId);
    const pathArray = document.head.querySelector("meta[name='cwps-meta-path-url']").content.split("/");
    let path = "";
    const titleArray = document.title.split("/");

    for(let i = 0;i < titleArray.length; i++) {
        let title = titleArray[i];
        path = path + "/" + pathArray[i];
        let anchor = document.createElement('a');
        anchor.appendChild(document.createTextNode(title));
        anchor.href = path;
        var li = document.createElement('li');
        li.appendChild(anchor);
        breadCrumbs.appendChild(li);
    }
    
 });