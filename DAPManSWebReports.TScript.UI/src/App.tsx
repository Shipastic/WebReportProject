import React, { useState, useEffect         } from 'react';
import { Container, Content,Footer, FlexboxGrid} from 'rsuite';
import { Outlet, BrowserRouter              } from 'react-router-dom';
import HeaderComponent     from './Components/HeaderComponent/HeaderComponent';
import AppRoutes           from './Components/Routes/AppRoutes';
import ContentComponent    from './Components/ContentComponent/ContentComponent';
import { UserProvider, useUser} from './Components/UserContext/UserContext';
import Breadcrumbs         from './Models/Breadcrumbs';
import { FolderDetail }    from './Models/FolderDetail';
import FooterPage          from './pages/FooterPage';
import config from './Utils/config';
import tokenService  from './Services/tokenService';
import logo               from './assets/css/logo.jpg';

import './App.css';
import ReportErrorComponent from './Components/ReportErrorComponent/ReportErrorComponent';

interface HomePageProps {
    onSelect?: (eventKey: any) => void;
    activeKey?: string;
    [key: string]: any;
}

const App: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) =>
{
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [activeFolderName, setActiveFolderName] = useState<string | null>(null);
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(true);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    const [breadcrumbs, setBreadcrumbs] = useState<string[]>(['Main']);

    const { user } = useUser() ?? {};

    const [backgroundImage, setBackgroundImage] = useState('./assets/first.jpg');

    const [showForm, setShowForm] = useState(false); 


    const handleReportClick = () => {
        setShowForm(!showForm); 
    };
    const fetchItems = async () => {
        const headers = tokenService.getAuthHeaders();
        try {
            const response = await fetch(`${config.ApiBaseUrlDev}/menu/parents`, { headers:headers });
            if (response.ok) {
                const data = await response.json();
                setItems(data);
            } else {
                throw new Error('Network response was not ok.');
            }
        } catch (error) {
            console.error("There was an error fetching the reports: ", error);
        }
    };

    useEffect(() => {
        if (user) { 
          fetchItems();
        }
      }, [user]);
   
    const handleDropdownSelect = () => {
        setActiveFolderId(null);  
        setBreadcrumbs(['Main']);
    };

    const handleItemClick = (id: number) => {
        setActiveFolderId(id);     
    };
    const handleItemClickName = (name : string) => {
        setActiveFolderName(name); 
    }
    const updateBackgroundImage = (imagePath) => {
        setBackgroundImage(imagePath);
      };

    const handleDataViewClick = (dataviewid: number) => {
        setSelectedDataViewId(dataviewid);
    };

    const updateBreadcrumbs = (newBreadcrumb: string[]) => {
        setBreadcrumbs(newBreadcrumb);
    };
    const handlePageClick = () => {
        setBreadcrumbs(['Main/']);
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
                        handleItemClickName={handleItemClickName}
                        updateBreadcrumbs={updateBreadcrumbs}
                        handleDataViewClick={handleDataViewClick}
                        onSelect={onSelect}
                        items={items} 
                        logo={logo}
                    />        
                    <Content className="content" style={{ backgroundImage: `url(${backgroundImage})`, backgroundPosition: 'center' }}>
                        <FlexboxGrid >
                            <Breadcrumbs breadcrumbs={breadcrumbs} />                   
                        </FlexboxGrid>
                        <FlexboxGrid >
                            <ContentComponent selectedItem={activeFolderName} updateBackgroundImage={updateBackgroundImage} />
                        </FlexboxGrid>
                        <AppRoutes
                                 updateBreadcrumbs={updateBreadcrumbs}
                                 breadcrumbs={breadcrumbs}
                                 activeFolderId={activeFolderId}
                                 isFolderComponentVisible={isFolderComponentVisible}
                         />   
                         
                        <Outlet />
                        <FlexboxGrid justify="end" style={{marginTop:'360px'}} > 
                        <div style={{ 
                             display: 'flex',
                             justifyContent: 'flex-end',
                             padding: '10px',
                             width: '400px',
                             height: 'auto'
                         }}>
                                {showForm && <ReportErrorComponent/>} 
                            </div> 
                        </FlexboxGrid>
                    </Content>                 
                    <Footer className='custom-footer'>
                        <FooterPage handleReportClick={handleReportClick}/>
                    </Footer>                   
                </Container> 
                </div>                             
            </BrowserRouter>
    );
}
export default App;