import React from 'react';
import { Icon } from '@rsuite/icons';
import { FaHeart } from 'react-icons/fa';

const Footer = () => {
    return (
        <footer className="custom-header">
            <div style={{ textAlign: 'center', color: '#fff' }}>
                <p>Developed by [Shipelov]
                    <Icon as={FaHeart} style={{ color: 'red', marginLeft: '5px' }} />
                </p>
            </div>
        </footer>
    );
};
export default Footer;