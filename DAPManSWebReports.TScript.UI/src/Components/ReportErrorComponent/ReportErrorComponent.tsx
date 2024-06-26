import React, { useState } from 'react';
import { Button, Form, Message, Panel, Uploader, Input} from 'rsuite';
import config                              from '..//..//Utils/config';

interface ReportError {
  description : string;
  email       : string;
  file        : any; 
}

const ReportErrorComponent: React.FC= () => {
  const [description, setDescription] = useState<string>('');
  const [email, setEmail            ] = useState<string>('');
  const [file, setFile              ] = useState<unknown>(null);
  const [loading, setLoading        ] = useState<boolean>(false);
  const [showSuccess, setShowSuccess] = useState<boolean>(false);
  const [showError, setShowError    ] = useState<boolean>(false);
  
  const fetchReportError = async (reportError: ReportError) => {
    const formData = new FormData();
    formData.append('description', reportError.description);
    formData.append('email', reportError.email);
    if (reportError.file && reportError.file instanceof Blob) {
      formData.append('file', reportError.file);
    }
    formData.append('url', window.location.href);
    try {
      console.log('Sending request:', reportError);
      const response = await fetch(`${config.ApiBaseUrlDev}/senderror`, {
        method: 'POST',
      body: formData
      });
      console.log('Response status:', response.status);
      console.log('Response body:', await response.text());

      if (response.ok) {
        setShowSuccess(true);
        setShowError(false);
      } else {
        setShowSuccess(false);
        setShowError(true);
      }
    } catch (error) {
      console.error('Error submitting report:', error);
      setShowSuccess(false);
      setShowError(true);
    } finally {
      setLoading(false);
    }
  };
  const handleSubmit = async () => {
    const reportError: ReportError = {
      description,
      email,
      file
    };

    setLoading(true);
    await fetchReportError(reportError);
  };

  return (    
    <Panel header={<h3 style={{ textAlign: 'center', color: 'red' }}>Сообщить об ошибке</h3>} style={{ backgroundColor: 'rgba(31,97,141, 0.4)' }} bordered>
      <Form fluid>
        <Form.Group controlId="description">
          <Form.ControlLabel>Описание ошибки</Form.ControlLabel>
          <Input
            as="textarea"
            value={description}
            onChange={value => setDescription(value)}
            rows={5}
            placeholder="Пожалуйста, опишите проблему, с которой вы столкнулись"
            required
          />
        </Form.Group>
        <Form.Group controlId="email">
          <Form.ControlLabel>Email (необязательно)</Form.ControlLabel>
          <Input
            type="email"
            value={email}
            onChange={value => setEmail(value)}
          />
        </Form.Group>
        <Form.Group controlId="file">
          <Form.ControlLabel>Скриншот ошибки (необязательно)</Form.ControlLabel>
          <Uploader
            listType="picture-text"
            action="" 
            autoUpload={false}
            onChange={(fileList) => setFile(fileList[0]?.blobFile)}
          />
        </Form.Group>
        <Form.Group>
          <Button appearance="primary" onClick={handleSubmit} loading={loading}>Отправить</Button>
        </Form.Group>
      </Form>
      {showSuccess && <Message type="success">Спасибо за обратную связь!</Message>}
      <div style={{marginTop:'10px'}}>
        {showError && <Message showIcon header="Ошибка!" type="error">Произошла ошибка при отправке. Пожалуйста, попробуйте еще раз.</Message>}
      </div>
    </Panel>
  );
};

export default ReportErrorComponent;