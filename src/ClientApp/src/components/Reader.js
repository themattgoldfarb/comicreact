import React, { useState } from 'react';
import { useLoaderData } from 'react-router-dom';
import useEventListener from "@use-it/event-listener";
import {Container} from 'react-bootstrap';
import './Reader.css';

export async function loader({params}) {
    const response = await fetch('comic/getcomic/'+params.title);
    const comicbook = await response.json();
    return {comicbook};
  }

const useKeyDown = (actions) => {
  useEventListener("keydown", (event) => {
    switch (event.key) {
      case "ArrowLeft":
        actions.prevPage();
        break;
      case "ArrowRight":
        actions.nextPage();
        break;
      case "h":
        actions.changeFit("height");
        break;
      case "w":
        actions.changeFit("width");
        break;
      case "b":
        actions.changeFit("both");
        break;
      case "s":
        actions.changeDisp("single");
        break;
      case "d":
        actions.changeDisp("double");
        break;
      default:
        break;
    }
  });
}


function Page(props) {
  let vis=props.page === props.currentPage ? "current" : "hidden";
  if (props.disp === "double" && props.page === props.currentPage + 1) {
    vis = "second";
  }

  let classes = vis + " fit-"+props.fit;
  if (props.fit === "height") {
    classes += " flex-grow-1";
  }
  return <img class={classes} 
              src={"comic/page/"+props.comicbook.title+"/"+props.page}/>;
}


export function Reader() {
  const {comicbook} = useLoaderData();
  const [current, setCurrent] = useState({
    page: 0,
    fit: "both",
    disp: "single"
  });

  const renderPages = (pageCount) => {
    let pages = [];
    for (let i = 0; i < pageCount; i++) {
      pages.push(
        <Page comicbook={comicbook}
              page={i}
              fit={current.fit}
              disp={current.disp}
              currentPage={current.page}/>);
    }
    return pages;
  }

  const actions = {
    nextPage: () => {
      setCurrent({
        page: current.page < comicbook.pageCount ? current.page + 1 : 0,
        fit: current.fit,
        disp: current.disp
      });
    },
    prevPage: () => {
      setCurrent({
        page: current.page > 0 ? current.page - 1 : comicbook.pageCount,
        fit: current.fit,
        disp: current.disp
      });
    },
    changeFit: (newfit) => {
      setCurrent({
        page: current.page,
        fit: newfit,
        disp: current.disp
      });
    },
    changeDisp: (newdisp) => {
      setCurrent({
        page: current.page,
        fit: current.fit,
        disp: newdisp
      });
    }
  };

  const key = useKeyDown(actions);
  
  return (
    <div>
      <Container 
          fluid 
          className={"reader-container cont-"+current.disp}
          onClick={actions.nextPage} >
        {renderPages(comicbook.pageCount)}
      </Container>
    </div>
    
  );
}
