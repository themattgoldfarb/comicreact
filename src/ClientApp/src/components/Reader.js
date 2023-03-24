import React, { useState } from 'react';
import { useLoaderData } from 'react-router-dom';
import './Reader.css';

export async function loader({params}) {
    const response = await fetch('comic/getcomic/'+params.title);
    const comicbook = await response.json();
    return {comicbook};
  }

function Page(props) {
  if (props.page === props.currentPage) {
    return <img class="current" 
                src={"comic/page/"+props.comicbook.title+"/"+props.page}/>;
  }
  return <img class="hidden"
              src={"comic/page/"+props.comicbook.title+"/"+props.page}/>;
}


export function Reader() {
  const {comicbook} = useLoaderData();
  const [current, setCurrent] = useState({page: 0});

  //const comicbook = { title: "test" };
  const renderPages = (pageCount) => {
    let pages = [];
    for (let i = 0; i < pageCount; i++) {
      pages.push(<Page comicbook={comicbook} page={i} currentPage={current.page}/>);
    }
    return pages;
  }

  const nextPage = () => {
    if (current.page < comicbook.pageCount) {
      setCurrent({page: current.page+1});
    } else {
      setCurrent({page: 0});
    }
  }
  
  return (
    <div 
      onClick={nextPage}
    >
      Hello 
      {comicbook.title}
      {renderPages(comicbook.pageCount)}
    </div>
    
  );


  //constructor (props) {
    //super(props);
    //comic = useLoaderData();
    //this.state = { 
      //title: this.props.value,
      //comic: [],
      //loading: true,
    //};
  //}

  //componentDidMount() {
    //this.populateComic();
  //}

  //static renderLoading() {
    //return (
      //<div>
      //this.state.title
        //<h3>Loading... </h3>
      //</div>
    //);
  //}

  //static renderComic(comic) {
    //<div>

      //this.state.title
    //</div>
  //}

  //render() {
    //let contents = this.state.loading
      //? Reader.renderLoading()
      //: Reader.renderComic(this.state.comic);

    //return (
      //<div>
        //{contents}
      //</div>
    //);
  //}

}
