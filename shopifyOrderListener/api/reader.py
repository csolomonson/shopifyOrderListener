import os

import requests

api_id = os.getenv("M1_API_ID", "")
api_key = os.getenv("M1_API_KEY", "")
headers = {
    "Accept": "application/json",
    "Authorization": f"apikey {api_id}:{api_key}",
}

url_stem = "http://192.168.1.200:1937/api/ERP/"

def get_request(table_name, parameters=None, columns=None, num_responses=1):
    if not api_id or not api_key:
        raise RuntimeError("M1_API_ID and M1_API_KEY must be configured")
    url = f'{url_stem}{table_name}'
    params = []
    if isinstance(parameters, list):
        for p in parameters:
            params.append(('filter', p))
    elif parameters:
        params.append(('filter', parameters))
    response = requests.get(url, headers=headers, params=params)
    response.raise_for_status()
    data = response.json()
    if data['recordCount'] == 0:
        print('no records')
        return None
    
    ans = []
    for i in range(min(num_responses, data['recordCount'])):
        record = []
        if columns and isinstance(columns, list):
            for c in columns:
                record.append(data['returnObject'][i][c])
            record = tuple(record)
        elif columns:
            record = data['returnObject'][i][columns]
        else:
            record = data['returnObject'][i]
        ans.append(record)
        

    if len(ans) == 1:
       return ans[0]
    return ans

def get_next_id(table_name):
    return get_request('NextIDs', f'xanTable[eq]{table_name}', 'xanNextID')

def get_customer_by_email(email):
    return get_request('Organizations', f'cmoEmailAddress[eq]{email}', 'cmoOrganizationID')

def get_organization_locations(org_id, address, zip_code):
    locations = get_request('OrganizationLocations', [f'cmlOrganizationID[eq]{org_id}', f'cmlPostCode[eq]{zip_code}'], ['cmlOrganizationLocationID', 'cmlAddressLine1'])
