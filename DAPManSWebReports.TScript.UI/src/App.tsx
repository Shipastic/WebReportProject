import React, { useState, useEffect } from 'react';
import CogIcon from '@rsuite/icons/legacy/Cog';
import CharacterAuthorizeIcon from '@rsuite/icons/CharacterAuthorize';
import { Container, Header, Content, Navbar, Nav, Dropdown} from 'rsuite';
import './App.css';
import './App.scss';
//import 'rsuite/dist/rsuite.min.css';
import ChildFolderComponent from './Components/ChildFolderComponent';
import QueryViewComponent from './Components/QueryViewComponent';
import Breadcrumbs  from './Models/Breadcrumbs';
import { FolderDetail } from './Models/FolderDetail';
import { Outlet, BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Home         from './pages/Home';
import About from './pages/About';
import Footer from './pages/Footer';
import Settings from './pages/Settings';
import LogIn from './pages/LogIn';
import NotFound from './pages/NotFound';
import logo from './assets/css/logo.jpg';
import Background from './assets/Background_theme.jpg';
import ExcelIcon from './assets/icons-excel-48.png';
import FolderIcon from './assets/icons-folder-32.png';

interface HomePageProps {
    onSelect?: (eventKey: any) => void;
    activeKey?: string;
    [key: string]: any;
}

const App: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) =>
{
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(false);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    const [breadcrumbs, setBreadcrumbs] = useState<string[]>(['Main']);

    const contentStyle = {
        backgroundImage: `url(${Background})`,
        backgroundSize: 'cover',
        backgroundRepeat: 'no-Repeat',
        backgroundPosition: 'center',
        width: '100vw',
        height: '100vh', // ”бедитесь, что высота достаточно больша€, чтобы отображать картинку
        color: 'white', // ƒл€ контрастного текста на фоне
    };

    useEffect(() => {
        fetch('https://localhost:7263/api/menu/parents')
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then(data => setItems(data))
            .catch(error => console.error("There was an error fetching the reports: ", error));
    }, []);
   
    const handleDropdownSelect = () => {
        setFolderComponentVisible(!isFolderComponentVisible);
        setActiveFolderId(null);  
        setBreadcrumbs(['Main']);
    };

    const handleItemClick = (id: number) => {
        setActiveFolderId(id);   
    };
    const handleDataViewClick = (dataviewid: number) => {
        setSelectedDataViewId(dataviewid);
    };

    const updateBreadcrumbs = (newBreadcrumb: string[]) => {
        setBreadcrumbs(newBreadcrumb);
    };
    const handlePageClick = () => {
        setBreadcrumbs(['Main']);
    };
    return (
        <BrowserRouter>          
        <div id="root">          
            <Container >
                    <Header className="custom-header" style={{ padding: '5px 5px 60px 0px', height: '30px' }}>
                        <Navbar {...props} appearance="subtle">
                            <Navbar.Header>
                                <Navbar.Brand className="navbar-brand logo" href="#" >
                                    <img
                                        src={logo}
                                        alt="logo"
                                        style={{ padding: '0px 0px 15px 5px', height: '30px' }} />
                                </Navbar.Brand>
                                <Navbar.Brand href="/" className="custom-nav-item">Main</Navbar.Brand>
                            </Navbar.Header>
                            <Navbar.Body>
                            <Nav onSelect={onSelect} activeKey={activeKey}>

                                    <Nav.Item onClick={handlePageClick} className="custom-nav-item"><Link to="/home">Home</Link></Nav.Item> 
                                    <Nav.Item onClick={handlePageClick} className="custom-nav-item"><Link to="/about">About</Link></Nav.Item>
                                    <Dropdown.Menu onSelect={handleDropdownSelect} title={"Select Plant"} className="custom-dropdown-menu">
                                        <Dropdown.Item className="custom-dropdown-item">Plants name:</Dropdown.Item>
                                {items?.childFolders.map(childFolder => (
                                    <Dropdown.Item
                                        key={childFolder.id}
                                        eventKey={childFolder.id}
                                        onSelect={() => handleItemClick(childFolder.id)}
                                    >
                                        <img
                                            src={FolderIcon}
                                            alt="foldericon"

                                            style={{ padding: '0px 10px 5px 15px', height: '30px', cursor: 'pointer', margin: '0px 10px 1px 15px' }}                                          
                                        />   
                                        {childFolder.name}
                                    </Dropdown.Item>
                                ))}
                                    <Dropdown.Item>Reports:</Dropdown.Item>
                                {items?.dataviews.map(dataview => (
                                    <Dropdown.Item
                                        key={dataview.id}
                                        onClick={() => handleDataViewClick(dataview.id)}
                                        className="custom-dropdown-item"
                                    >
                                        <img
                                            src={ExcelIcon}
                                            alt="excelicon"

                                            style={{ padding: '0px 10px 5px 15px', height: '30px', cursor: 'pointer', margin: '0px 10px 1px 15px' }}
                                            onClick={() => console.log('Download clicked')} // ƒобавить логику здесь
                                        />    
                                        {dataview.name}
                                    </Dropdown.Item>
                                ))}
                                    </Dropdown.Menu> 
                        </Nav>
                            <Nav pullRight>
                                <Nav.Item
                                    onClick={handlePageClick}
                                        icon={<CharacterAuthorizeIcon />} className="custom-nav-item">
                                        <Link to="/login" className="nav-link">
                                        LogIn
                                    </Link>
                                </Nav.Item>
                                    <Nav.Item icon={<CogIcon />} className="custom-nav-item">
                                    <Link to="/settings" className="nav-link">
                                        Settings
                                    </Link>
                                </Nav.Item>
                                </Nav> 
                            </Navbar.Body>
                        </Navbar >                      
                    </Header>
                    <Content className="content" >
                        <Breadcrumbs breadcrumbs={breadcrumbs} />
                        {selectedDataViewId !== null && (
                            <div className="query-view-container">
                                <QueryViewComponent dataviewid={selectedDataViewId} path={'/'} updateBreadcrumbs={updateBreadcrumbs} breadcrumbs={breadcrumbs} />
                            </div>
                        )}
                        <Routes>
                            <Route path="/home" element={<Home
                                                            path=""
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="/login" element={<LogIn
                                                            path="/login"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />     
                            <Route path="/about" element={<About
                                                            path="/about"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="*"      element={<NotFound
                                                            path="NotFoundPAge"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="/"      element={activeFolderId !== null && (
                                                     <ChildFolderComponent
                                                            parentid={activeFolderId}
                                                            path=""
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs} />)} />
                            <Route path="/settings" element={<Settings
                                                                path=""
                                                                updateBreadcrumbs={updateBreadcrumbs}
                                                                breadcrumbs={breadcrumbs}        />} />
                        </Routes>    
                        <Outlet />
                    </Content>
                    <Footer/>
            </Container>                   
            </div>
        </BrowserRouter>
    );
}
export default App;