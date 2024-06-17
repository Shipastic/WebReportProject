import React, { useState, useEffect         } from 'react';
import { Container, Content, Sidebar, Footer} from 'rsuite';
import { Outlet, BrowserRouter              } from 'react-router-dom';

import SideBarComponent   from './Components/SidebarComponent';
import HeaderComponent    from './Components/HeaderComponent';
import AppRoutes          from './Components/AppRoutes';

import Breadcrumbs        from './Models/Breadcrumbs';
import { FolderDetail }   from './Models/FolderDetail';

import FooterPage         from './pages/FooterPage';

import logo               from './assets/css/logo.jpg';

import './App.css';

interface HomePageProps {
    onSelect?: (eventKey: any) => void;
    activeKey?: string;
    [key: string]: any;
}

const App: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) =>
{
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(true);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    const [breadcrumbs, setBreadcrumbs] = useState<string[]>(['Main']);

    const [activeKeySideBar, setActiveKey] = useState('1');
    const [openKeys, setOpenKeys] = useState(['1', '2']);
    const [expanded, setExpand] = useState(true);

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
            <Container className='container'>          
                     <HeaderComponent 
                            className="custom-header"
                            props={props}
                            activeKey={activeKey}
                            handlePageClick={handlePageClick}
                            handleDropdownSelect={handleDropdownSelect}
                            handleItemClick={handleItemClick}
                            updateBreadcrumbs={updateBreadcrumbs}
                            handleDataViewClick={handleDataViewClick}
                            onSelect={onSelect}
                            items={items} 
                            logo={logo}/>
                    <Container className='container'>
                        <Sidebar className='sidebar' style={{flex:'0 1'}}>
                            <SideBarComponent                            
                                  activeKeySideBar={setActiveKey}
                                  openKeys={openKeys}
                                  onSelect={setActiveKey}
                                  onOpenChange={setOpenKeys}
                                  expanded={expanded}
                                  onExpand={setExpand}
                                  appearance="subtle"/>
                        </Sidebar>
                        <Content className="content" >
                        <Breadcrumbs breadcrumbs={breadcrumbs} />
                        {/*
                        {selectedDataViewId !== null && (
                            <div className="query-view-container">
                                <QueryViewComponent dataviewid={selectedDataViewId} path={'/'} updateBreadcrumbs={updateBreadcrumbs} breadcrumbs={breadcrumbs} />
                            </div>
                        )}
                            */}
                       <AppRoutes
                                updateBreadcrumbs={updateBreadcrumbs}
                                breadcrumbs={breadcrumbs}
                                activeFolderId={activeFolderId}
                                isFolderComponentVisible={isFolderComponentVisible}
                        />   
                        <Outlet />
                        </Content>
                    </Container>
                    <Footer className='footer'>
                        <FooterPage/>
                    </Footer>                   
            </Container> 
            </div>                             
        </BrowserRouter>
    );
}
export default App;