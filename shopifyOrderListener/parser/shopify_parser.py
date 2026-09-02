#TEST_BODY = '''
'''.button__cell { background: #1990C6; }
a, a:hover, a:active, a:visited { color: #1990C6; }
@media print{
body {
color: black !important;
}

.subtitle-lines,
.subtotal-line__title,
.subtotal-line__value {
padding: 0 !important;
margin: 0 !important;
}

.subtotal-table {
margin: 0 !important;
}
}

Joshua Dodsworth placed order S-1738 on Jul
14 at 4:59 pm.

View order
( https://tnicxt-br.myshopify.com/admin/orders/7337326510363?syclid=e1fc04cb-cb10-40a3-9d32-89a4acc6dff8 )


Order summary

Fitting, #8AN ORB to 3/4" Hose

$24.41 × 3

Black •  SKU: WPM34S

$73.23

Hose Adapter, #20AN to 1.75" Slip Hose

$27.36 × 3

Black •  SKU: WA20175S

$82.08

Subtotal

$155.31

Shipping
(FedEx Ground Home Delivery)

$23.86

Total

$179.17 USD

Payment processing method

Shopify payments

Delivery method

FedEx Ground Home Delivery

Shipping address

Joshua Dodsworth

1870 County Road 2450

Huntsville,
Missouri
65259

United States

6604157909

Customer email

josh@americanironoffroad.com

Shopify

151 O'Connor Street, Ground floor, Ottawa, ON, K2P 2L8'''

TEST_BODY = '''
.button__cell { background: #1990C6; }
a, a:hover, a:active, a:visited { color: #1990C6; }
@media print{
body {
color: black !important;
}

.subtitle-lines,
.subtotal-line__title,
.subtotal-line__value {
padding: 0 !important;
margin: 0 !important;
}

.subtotal-table {
margin: 0 !important;
}
}

MICHAEL PRUITT placed order S-1734 on Jul
14 at 7:01 am.

View order
( https://tnicxt-br.myshopify.com/admin/orders/7336502395163?syclid=94fd0e1e-4259-4316-8652-20c3d31381ce )


Order summary

90 Degree Adapter, WN Style

$128.56 × 1

Black •  SKU: WN2090S

$128.56

Extension, WN Style, 2.6" OAL

$34.98 × 1

Black •  SKU: WN2000S

$34.98

Fitting, WN Style, for #20AN hose

$26.72 × 1

Black •  SKU: WN0041S

$26.72

Subtotal

$190.26

Shipping
(FedEx Ground)

$17.20

Total

$207.46 USD

Payment processing method

Shopify payments

Delivery method

FedEx Ground

Shipping address

michael pruitt

centralgarage repairs

1309 s commerce st

bremond,
Texas
76629

United States

+12547467012

Customer email

CENTRALGARAGEREPAIRS@GMAIL.COM

Shopify

151 O'Connor Street, Ground floor, Ottawa, ON, K2P 2L8'''
from api.reader import get_customer_by_email

def parse_shopify(message):
    #print(message.text.split('\n'))
    subject = message.subject
    order_id = subject.split('Order ')[1][:6]
    body = message.text.replace('\r', '')
    lines = parse_lines(body)
    customer = parse_customer(body)
    return {'Customer PO': order_id,
            'Lines': lines,
            'Customer': customer}

def parse_lines(email_body):  
    body_lines =  email_body.split('\n')
    summary = body_lines[body_lines.index('Order summary'):]

    line_lines = summary[2: summary.index('Subtotal')]
    lines = []
    for i in range(len(line_lines) // 8):
        line = line_lines[8*i:8*(i+1)]
        #print(i, line)
        description = line[0]
        unit_price = line[2].split(' × ')[0][1:]
        line_quantity = line[2].split(' × ')[1]
        sku = line[4].split('SKU: ')[1]
        line_total = line[6][1:]

        lines.append({'Line ID'      : i,
                     'Description'  : description, 
                     'Unit Price'   : unit_price,
                     'Line Quantity': line_quantity,
                     'SKU'          : sku,
                     'Line Total'   : line_total})
    return lines


def parse_customer(body):
    delivery_method = body.split('Shipping address')[0].split('\n')[-3]
    is_business = not 'Home' in delivery_method
    customer_body = body.split('Shipping address')[-1].split('\n')
    #print(customer_body)

    if not is_business:
        name = customer_body[2]
        business = ''
        address_line_one = customer_body[4]
        address_line_two = customer_body[5]
        city = customer_body[6]
        state = customer_body[7]
        zip_code = customer_body[8]
        country = customer_body[10]
        phone = customer_body[12]
        email = customer_body[customer_body.index('Customer email')+2]
        
    else:
        name = customer_body[2]
        business = customer_body[4]
        address_line_one = customer_body[6]
        address_line_two = customer_body[7]
        city = customer_body[8]
        state = customer_body[9]
        zip_code = customer_body[10]
        country = customer_body[12]
        phone = customer_body[14]
        email = customer_body[customer_body.index('Customer email')+2]
    print(get_customer_by_email(email))
    return {'Is Business' : is_business,
            'Name': name,
            'Business': business,
            'Address Line One': address_line_one,
            'Address Line Two': address_line_two,
            'City': city,
            'State': state,
            'Zip': zip_code,
            'Country': country,
            'Phone': phone,
            'Email' : email}
    
if __name__ == '__main__':
    parse_lines(TEST_BODY)
    print(parse_customer(TEST_BODY))
    
