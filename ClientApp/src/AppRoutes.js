import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Library } from "./components/Library";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/library',
    element: <Library />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
