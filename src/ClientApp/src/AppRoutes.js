import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Library } from "./components/Library";
import { Reader, loader as readerLoader } from "./components/Reader";
import App from "./App";
import {
  createBrowserRouter,
} from "react-router-dom";

const AppRoutes = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        index: true,
        element: <Home />
      },
      {
        path: '/library',
        element: <Library />
      },
      {
        path: '/reader/:title',
        element: <Reader />,
        loader: readerLoader,
      },
      {
        path: '/counter',
        element: <Counter />
      },
      {
        path: '/fetch-data',
        element: <FetchData />
      }
    ],
  },
]);

export default AppRoutes;
