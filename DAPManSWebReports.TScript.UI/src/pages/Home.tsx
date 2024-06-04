//import React, { useState, useEffect } from 'react';
//import CogIcon from '@rsuite/icons/legacy/Cog';
//import { Container, Header, Content, Navbar, Nav, Dropdown } from 'rsuite';
//import './App.css';
//import 'rsuite/dist/rsuite.min.css';
//import Footer from "./pages/Footer";
//import ChildFolderComponent from './Components/ChildFolderComponent';
//import { FolderDetail } from './Models/FolderDetail';
//import { Routes, Route, Link, useNavigate, Outlet, BrowserRouter } from 'react-router-dom';
//import AppRoutes from './pages/AppRoutes';
//import Home from './pages/Home';
//import About from './pages/About';
//import ShowDataView from './pages/ShowDataView';
//import LogIn from './pages/LogIn';
//import NotFound from './pages/NotFound';

//interface HomePageProps {
//    onSelect?: (eventKey: any) => void;
//    activeKey?: string;
//    [key: string]: any;
//}

//const App: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) => {
//    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
//    const [items, setItems] = useState<FolderDetail | null>(null);
//    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(false);
//    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

//    const navigate = useNavigate();

//    const handleShowDataViewClick = () => {
//        navigate('/dataview');
//    };

//    useEffect(() => {
//        fetch('https://localhost:7263/api/menu/parents')
//            .then(response => {
//                if (response.ok) {
//                    return response.json();
//                }
//                throw new Error('Network response was not ok.');
//            })
//            .then(data => setItems(data))
//            .catch(error => console.error("There was an error fetching the reports: ", error));
//    }, []);

//    const handleDropdownSelect = () => {
//        setFolderComponentVisible(!isFolderComponentVisible);
//    };

//    const handleItemClick = (id: number) => {
//        setActiveFolderId(id);
//    };
//    const handleDataViewClick = (dataviewid: number) => {
//        setSelectedDataViewId(dataviewid);
//    };

//    return (
//        <div id="root">
//            <Container className="container">
//                <Header className="header">
//                    <Navbar {...props}>
//                        <Navbar.Brand href="/">Main</Navbar.Brand>
//                        <Nav onSelect={onSelect} activeKey={activeKey}>
//                            <Nav.Item><Link to="/login" >LogIn</Link></Nav.Item>
//                            <Nav.Item><Link to="/dataview">Registration</Link></Nav.Item>
//                            <Dropdown.Menu onSelect={handleDropdownSelect} title={"Select Plant"}>
//                                {items?.childFolders.map(childFolder => (
//                                    <Dropdown.Item
//                                        key={childFolder.id}
//                                        eventKey={childFolder.id}
//                                        onSelect={() => handleItemClick(childFolder.id)}
//                                    >
//                                        {childFolder.name}
//                                    </Dropdown.Item>
//                                ))}
//                                <hr />
//                                {items?.dataviews.map(dataview => (
//                                    <Dropdown.Item
//                                        key={dataview.id}
//                                        onClick={() => handleDataViewClick(dataview.id)}
//                                    >
//                                        {dataview.name}
//                                    </Dropdown.Item>
//                                ))}
//                            </Dropdown.Menu>
//                        </Nav>
//                        <Nav pullRight>
//                            <Nav.Item icon={<CogIcon />}>Settings</Nav.Item>
//                        </Nav>
//                    </Navbar >
//                </Header>
//                <Content className="content">
//                    <Outlet />
//                    {activeFolderId !== null && (
//                        <ChildFolderComponent
//                            parentid={activeFolderId} />
//                    )}

//                </Content>
//                <Footer />
//            </Container>
//            <Routes>
//                <Route index element={<Home />} />
//                <Route path="/login" element={<LogIn />} />
//                <Route path="/dataview" element={<ShowDataView />} />
//                <Route path="*" element={<NotFound />} />
//            </Routes>
//        </div>
//    );
//}
//export default App;

import Reactfrom from 'react';
import { Container, Content } from 'rsuite';
import Breadcrumbs from '../Models/Breadcrumbs';
import '../App.css';
import 'rsuite/dist/rsuite.min.css';

interface Props {
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}

const Home: React.FC<Props> = ({ path, updateBreadcrumbs, breadcrumbs }) =>
{
    let pathArray: string[] = [];
    let newBreadcrumbs: any[] = [];


    const handleHomeClick = async () => {       
        try
        {            
            const newPath = path ? `${path}/Home` : 'ErrorPage';
            newBreadcrumbs = [...breadcrumbs, 'Home'];
            pathArray.push(path, newPath);
            updateBreadcrumbs(newBreadcrumbs);
        }
        catch (error)
        {
            console.error("Ошибка при формировании breadcrumbs", error);
        }
    };

    return (            
        <Container className="container">
            <div onClick ={handleHomeClick}>
                  <Content className="content">
                    <p>Home Page Content!</p>
                </Content>
            </div>
            </Container>
    );
};
export default Home;