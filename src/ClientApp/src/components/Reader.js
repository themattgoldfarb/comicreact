import React, { setState } from 'react';
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import './Library.css';
import { useLoaderData } from 'react-router-dom';

export async function loader({params}) {
    const response = await fetch('comic/getcomic/'+params.title);
    const comicbook = await response.json();
    return {comicbook};
  }

function Page(props) {
  return <img src={"comic/page/"+props.comicbook.title+"/"+props.page}/>;
}


export function Reader() {
  const {comicbook} = useLoaderData();

  //const comicbook = { title: "test" };
  const renderPages = (pageCount) => {
    let pages = [];
    for (let i = 0; i < pageCount; i++) {
      pages.push(<Page comicbook={comicbook} page={i}/>);
    }
    return pages;
  }
  
  return (
    <div> 
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
